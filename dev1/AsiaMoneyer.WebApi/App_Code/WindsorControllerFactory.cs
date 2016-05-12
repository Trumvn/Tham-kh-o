using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace AsiaMoneyer.WebApi
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException(String.Format("Type requested is not a controller: {0}", controllerType.Name), "controllerType");
            }

            /*try
            {
                if (this.container.IsRegistered(controllerType))
                {
                    controller = this.container.Resolve(controllerType) as IController;
                }
                else
                {
                    controller = base.GetControllerInstance(requestContext, controllerType);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Error resolving controller {0}", controllerType.Name, ex));
            }*/

            return (IController)_kernel.Resolve(controllerType);
        }
    }
}