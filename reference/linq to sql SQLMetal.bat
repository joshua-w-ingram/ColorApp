cd "c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC"

call vcvarsall.bat

sqlmetal "C:\SQLCompact_Temp\colordataDb.sdf" /code:"c:\SQLCompact_Temp\cdDb_DataClasses.cs" /language:csharp /namespace:Color_Data_3._0.db /context:cdDb_DataContext /pluralize

%SystemRoot%\notepad.exe "C:\SQLCompact_Temp\cdDb_DataClasses.cs"