echo Delete old files...
del ".\..\wwwroot\*.*" /s /f /q
echo Done!

npx vue-cli-service build --no-clean --dest .\..\wwwroot