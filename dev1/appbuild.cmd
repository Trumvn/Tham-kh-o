echo EMMS AUTOMATION BUILD
echo RUN BUILD.XML
%windir%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe build.xml /p:DeployFolder=%USERPROFILE%\Documents\Temp\AsiaMoneyerBuild\
