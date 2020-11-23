ECHO @off
ECHO "Bulding Resources.."
call C:\dev\csharp\nephilim\Nephilim.BuildTool\bin\Debug\netcoreapp3.1\Nephilim.BuildTool.exe assets C:\dev\csharp\nephilim\Nephilim.Sandbox\Resources C:\dev\csharp\nephilim\Nephilim.Desktop\Resources
ECHO "Running Game..."
call dotnet run