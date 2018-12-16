cd bt-client
npm run build
copy .\build\static\js\*.js ..\www\static\js\main.js
copy .\build\static\css\*.css ..\www\static\css\main.css
cd ..
cordova build