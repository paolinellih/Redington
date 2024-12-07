# Use an official Node.js image as a base
FROM node:20 AS build

# Set working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json into the container
COPY CalculatorAPP/package*.json ./

# Install dependencies
RUN npm install

# Accept build arguments
# ARG VITE_CALCULATOR_API_URL $VITE_CALCULATOR_API_URL
ENV VITE_CALCULATOR_API_URL="http://calculator-api:5000"

# Copy the rest of the application source code
COPY CalculatorAPP/ .

# Build the React application
RUN npm run build

# Use a lightweight web server for serving the build files
FROM nginx:alpine

# Copy build output from the previous stage to the Nginx web root
COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]