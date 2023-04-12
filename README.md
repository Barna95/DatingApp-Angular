<h1 align="center"> Dating application (Under development) </h1>

<h4 align="center">A dating applicaiton that will help you find your best match in our sped up world. <br><br><br>
<a href="https://github.com/Barna95/DatingApp-Angular/issues">Report Bug</a>
    ·
    <a href="https://github.com/Barna95/DatingApp-Angular/issues">Request Feature</a></h4>
    
## Prerequisites

- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup)
- [.NET 7.0 SDK - Windows x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.202-windows-x64-installer)
- [Node.js ](https://nodejs.org/en/download)
- Check the nodejs and npm version. They suppose to be Node >= 14.0.0 and npm >= 5.6
```powershell
node -v
```
```powershell
npm -v
```
- Install Angular
```powershell
npm install -g @angular/cli
```
- Install Rxjs
```powershell
npm install rxjs
```
### Run the backend
1.If you don't have all the packages installed, run in terminal:
```powershell
dotnet restore
```
2.Run the .sln in your IDE OR move into the rootfolder and use the following in your terminal:
```powershell
dotnet run
 ```

### Run the frontend
1. Make it sure that you are in the frontend folder.
2. In case the packages are missing:
 ```powershell
 npm install
 ```
3. Run the following in your terminal:
```powershell
 ng serve
 ```
