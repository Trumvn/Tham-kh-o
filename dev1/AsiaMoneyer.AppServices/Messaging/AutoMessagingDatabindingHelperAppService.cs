using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Messaging
{
    public class AutoMessagingDatabindingHelperAppService : AppServiceBase, IAutoMessagingDatabindingHelperAppService
    {
        public AutoMessagingDatabindingHelperAppService()
        {

        }

        public String bind(String source, Dictionary<string, string> values)
        {
            String result = source;
            if (!String.IsNullOrEmpty(source))
            {
                List<String> keys = Commons.Ultility.GetMatches(source);
                if (keys.Count > 0)
                {
                    // Get from dynamic binding
                    if (values != null)
                    {
                        List<String> removeKeys = new List<string>();
                        foreach (String key in keys)
                        {
                            if (values.ContainsKey(key))
                            {
                                result = result.Replace(key, values[key]);
                                removeKeys.Add(key);
                            }
                        }

                        foreach (String removeKey in removeKeys)
                        {
                            keys.Remove(removeKey);
                        }
                    }

                    // Get from database
                    List<AutoMessagingDataMapping> mapping = this.UnitOfWork.AutoMessagingDataMappingRepository.List.Where(x => keys.Contains(x.TokenKey)).ToList();

                    foreach (AutoMessagingDataMapping map in mapping)
                    {
                        if (!String.IsNullOrEmpty(map.Value))
                        {
                            if (values != null && values.ContainsKey(map.Value))
                            {
                                result = result.Replace(map.TokenKey, values[map.Value]);
                            }
                            else
                            {
                                result = result.Replace(map.TokenKey, map.Value);
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(map.SqlQuery))
                            {
                                string val = String.Empty;
                                if (!String.IsNullOrEmpty(map.RequiredColumnName) && values.ContainsKey(map.RequiredColumnName))
                                {
                                    val = values[map.RequiredColumnName];
                                }

                                string sqlQuery = String.Format(map.SqlQuery, val);

                                var queryResult = this.UnitOfWork._dbContextProvider.DbContext.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();

                                if (queryResult != null)
                                {
                                    result = result.Replace(map.TokenKey, queryResult.ToString());
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
