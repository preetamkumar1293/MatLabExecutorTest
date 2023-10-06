@echo off
REM Check if the MATLAB executable path is provided as an argument
if "%~1"=="" (
    echo Usage: run_matlab.bat "MATLAB_executable_path" "MATLAB_script_argument"
    exit /b 1
)

REM Check if the MATLAB script argument is provided as an argument
if "%~2"=="" (
    echo Usage: run_matlab.bat "MATLAB_executable_path" "MATLAB_script_argument"
    exit /b 1
)

REM Set the MATLAB executable path and script argument
set MATLAB_EXECUTABLE=%~1
set MATLAB_ARGUMENT=%~2

REM Run MATLAB with the provided argument
"%MATLAB_EXECUTABLE%" -r "%MATLAB_ARGUMENT%"

REM Exit the batch script
exit /b 0