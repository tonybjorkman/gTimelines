            // Chrome Driver was manually downloaded from https://sites.google.com/a/chromium.org/chromedriver/downloads
            // parameter "." will instruct to look for the chromedriver.exe in the current folder

            // Warning! Automatic downloads needs to be enabled in the Chrome preferences.
            // On dev computer this preference JSON file is located at:
            // /home/tony/.config/google-chrome/Default/Preferences
            // in there set "automatic_downloads":{} to
            // "automatic_downloads":1


Setting up the test project:
in testdir: dotnet new xunit
go down to parentfolder: dotnet add package xunit
(check that its possible to link xunit in the main-project)
(see that the xunit is now resolved in the test-project aswell)
in testdir: dotnet test