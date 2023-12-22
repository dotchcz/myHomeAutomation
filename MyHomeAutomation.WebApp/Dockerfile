#stage 1
#FROM node:14-bullseye-slim as node
##WORKDIR /app
#COPY ./MyHomeAutomation.WebApp/ .
#RUN npm install
#RUN npm run build --prod
#stage 2
#FROM nginx:alpine
#COPY --from=node /app/dist/my-home-automation.web-app /usr/share/nginx/html


# Stage 1: Node.js base image with just the dependencies needed
FROM node:14-bullseye-slim as node

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY ./MyHomeAutomation.WebApp/package*.json ./

# Install node modules
RUN npm install

# Copy the rest of the app
COPY ./MyHomeAutomation.WebApp/ .

# Build the Angular application in production mode
RUN npm run build --prod

# Stage 2: Nginx to serve the application
FROM nginx:alpine

# Copy the build output from stage 1
COPY --from=node /app/dist/my-home-automation.web-app /usr/share/nginx/html
