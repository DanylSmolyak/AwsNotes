# AWS Notes API — .NET + AppSync + DynamoDB

API_KEY_NAME x-api-key

API_KEY da2-6z2omgzs5nfe5ccx3bztiaudbu

GraphQL endpoint https://5f6fmrh2dvdcxlgi3lncxrupeq.appsync-api.eu-north-1.amazonaws.com/graphql

##  Опис

Проєкт є серверною частиною системи нотаток, реалізованою на .NET та розгорнутою в AWS з використанням таких сервісів:

- **AWS Lambda** — для реалізації бізнес-логіки (через функції на C#)
- **AWS AppSync** — як GraphQL API
- **Amazon DynamoDB** — як база даних

##  Сутності

- `User` — користувач
- `Note` — нотатка користувача


###  Користувачі:

- GetUser(id: ID!) — отримати користувача
- GetAllUsers — всі користувачі
- CreateUser(Username: String!, Email: String!) — створити користувача
- UpdateUser(Id: ID!, Username: String!, Email: String!) — оновити користувача
- DeleteUser(Id: ID!) — видалити користувача

###  Нотатки:

- CreateNote(UserId: ID!, Title: String!, Content: String!) — створити нотатку
- GetAllNotes: - всі записи
- GetNote(Id: ID!) - отримати запис
- GetNotesByUser(userId: ID!) — отримати нотатки користувача
- UpdateNote(Id: ID!, Title: String!, Content: String!) — оновити нотатку
- DeleteNote(Id: ID!) — видалити нотатку


####  CRUD операції через GraphQL (Postman / AppSync)

1) (GetUser)
    
    {
  "query": "query GetUser($id: ID!) { GetUser(Id: $id) { Success Message Data { Id Username Email CreatedAt UpdatedAt } } }",
  "variables": {
    "id": "USER_ID_HERE"
  }
}

2) (CreateUser)
   {
  "query": "mutation CreateUser($username: String!, $email: String!) { CreateUser(Username: $username, Email: $email) { Success Message Data { Id Username Email CreatedAt UpdatedAt } } }",
  "variables": {
    "username": "name",
    "email": "mail"
  }
}

3) (UpdateUser)
   {
  "query": "mutation UpdateUser($id: ID!, $username: String!, $email: String!) { UpdateUser(Id: $id, Username: $username, Email: $email) { Success Message Data { Id Username Email CreatedAt UpdatedAt } } }",
  "variables": {
    "id": "USER_ID_HERE",
    "username": "danylo_updated",
    "email": "new@example.com"
  }
}

4) (DeleteUser)
{
  "query": "mutation DeleteUser($id: ID!) { DeleteUser(Id: $id) { Success Message Data } }",
  "variables": {
    "id": "USER_ID_HERE"
  }
}

5) (CreateNote)

{
  "query": "mutation CreateNote($title: String!, $content: String!, $userId: ID!) { CreateNote(Title: $title, Content: $content, UserId: $userId) { Success Message Data { Id Title Content UserId CreatedAt UpdatedAt } } }",
  "variables": {
    "title": "My first note",
    "content": "Hello from AWS",
    "userId": "USER_ID_HERE"
  }
}

6) (GetNotesByUserId)
{
  "query": "query GetNotesByUserId($userId: ID!) { GetNotesByUserId(UserId: $userId) { Success Message Data { Id Title Content UserId CreatedAt UpdatedAt } } }",
  "variables": {
    "userId": "USER_ID_HERE"
  }
}

7) (UpdateNote)
{
  "query": "mutation UpdateNote($id: ID!, $title: String!, $content: String!) { UpdateNote(Id: $id, Title: $title, Content: $content) { Success Message Data { Id Title Content UserId CreatedAt UpdatedAt } } }",
  "variables": {
    "id": "NOTE_ID_HERE",
    "title": "Updated note title",
    "content": "Updated content"
  }
}

8) (DeleteNote)
{
  "query": "mutation DeleteNote($id: ID!) { DeleteNote(Id: $id) { Success Message Data } }",
  "variables": {
    "id": "NOTE_ID_HERE"
  }
}

9) (GetAllUsers)
{
  "query": "query GetAllUsers { GetAllUsers { Success Message Data { Id Username Email CreatedAt UpdatedAt } } }"
}

10) (GetAllNotes)
{
  "query": "query GetAllNotes { GetAllNotes { Success Message Data { Id Title Content UserId CreatedAt UpdatedAt } } }"
}




