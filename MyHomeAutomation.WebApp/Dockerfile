#stage 1
FROM node:latest as node
WORKDIR /app
COPY ./MyHomeAutomation.WebApp/ .
RUN npm install
RUN npm run build --prod
#stage 2
FROM nginx:alpine
COPY --from=node /app/dist/my-home-automation.web-app /usr/share/nginx/html