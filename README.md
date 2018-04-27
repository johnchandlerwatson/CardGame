# Card Game

> .net core web api with vue.js

## Dependencies:
* .net core 2.0
<br>https://www.microsoft.com/net/learn/get-started/macos
* npm
<br>https://www.npmjs.com/get-npm
* node
* vue-cli
```
    npm install vue-cli --global
```
* vue-drag-drop
```
    npm install --save vue-drag-drop
```
* aspnet-webpack (local to project)
```
    npm i aspnet-webpack
```



## Deploy
* dotnet publish -c Release
* docker build -t cards ./bin/release/netcoreapp2.0/publish
* docker tag cards registry.heroku.com/heart-of-the-cards/web
* docker push registry.heroku.com/heart-of-the-cards/web

(you might need to login)
* heroku login
* heroku container:login
