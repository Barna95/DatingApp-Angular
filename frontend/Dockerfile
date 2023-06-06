# Stage 1: Build the Angular app
FROM node:latest AS build
WORKDIR /usr/src/app
COPY package*.json ./
RUN npm install -g @angular/cli
RUN npm install --legacy-peer-deps
COPY . .
RUN ng build

# Stage 2: Serve the built app using NGINX
FROM nginx:latest
COPY --from=build /usr/src/app/dist/frontend /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

