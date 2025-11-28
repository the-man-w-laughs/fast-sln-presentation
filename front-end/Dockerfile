FROM node:lts-alpine

WORKDIR /front-end

# Копируем файлы зависимостей
COPY package*.json ./

# Устанавливаем зависимости проекта
RUN npm install

# Копируем весь проект
COPY . .

EXPOSE 3000

CMD ["npm", "start"]
