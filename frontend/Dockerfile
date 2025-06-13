FROM node:20-alpine AS builder

WORKDIR /app

COPY package.json package-lock.json ./

# RUN npm install --f --production
RUN npm i --production

COPY . .

EXPOSE 3000

# CMD ["npm", "run", "start"]

# Build the Next.js application
RUN npm run build

RUN npm run postbuild
# Install PM2 globally
RUN npm install pm2 -g

# Expose the port that the Next.js application will run on
EXPOSE 3000

# Start the Next.js application with PM2
CMD ["pm2-runtime", "npm", "--", "start"]