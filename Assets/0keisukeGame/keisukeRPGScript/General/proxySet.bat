@echo off
set /p httpProxy="Enter HTTP proxy (format: http://proxy.server.com:port): "
set /p httpsProxy="Enter HTTPS proxy (format: https://proxy.server.com:port): "

git config --global http.proxy %httpProxy%
git config --global https.proxy %httpsProxy%

echo.
echo Proxy settings updated for Git:
echo HTTP Proxy: %httpProxy%
echo HTTPS Proxy: %httpsProxy%
pause
