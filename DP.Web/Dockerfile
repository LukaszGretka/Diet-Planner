FROM node:18 AS build
WORKDIR /app

COPY ./package*.json ./
RUN npm install -g @angular/cli
RUN npm install

COPY ./ .

RUN ng build --configuration production

FROM nginx:alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY ./nginx-custom.conf /etc/nginx/conf.d/default.conf