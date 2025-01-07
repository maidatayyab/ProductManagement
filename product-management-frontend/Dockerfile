FROM node:18 AS build
WORKDIR /app
COPY product-management-frontend/package*.json ./
RUN npm install
COPY product-management-frontend/ ./
RUN npm run build

FROM nginx:alpine AS final
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
