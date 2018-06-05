# TestRunner
The program helps easily execute tests from job file. You should check your job file yourself. 
###### Job file creator comes later.

### 1)Execution format:
To execute program, type in command line:
testrunner.exe [-r count] [-j jobFileName]
```
Options :
-r count    runners count, buy default, it is calculated as number 
            of logical cores on the system. The maximum allowed number 
            should be twice as the default one
-j name     a CSV file that includes transcription of all the tests
```
### 2)Job file
A CSV file that includes transcription of all the tests to
execute. Every single line is dedicated to a single test run. Sample:
```
test ID,command
1,wget cnn.com --directory-prefix=cnn.com
```
The first line will be ignored. Delimiter is comma.

### 3)Known limitations:
TestRunner supports all command line commands and programs, except for those that require input.
If a Test uses a program, it should be put in execution directory. It's better if you try to execute the command manually from the TestRunner directory before.

### 4)Licenses : Just for internal use
