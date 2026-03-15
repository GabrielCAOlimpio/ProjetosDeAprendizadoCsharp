📘 Documentação para GitHub
# SocialMedia API

Backend de uma aplicação de rede social desenvolvido em **ASP.NET Core**.  
O projeto foi criado com o objetivo de estudar e aplicar conceitos avançados de **arquitetura backend, autenticação, modelagem de banco de dados e boas práticas de desenvolvimento com .NET**.

A API fornece todas as funcionalidades essenciais para o funcionamento de uma rede social, incluindo **autenticação segura, gerenciamento de usuários, posts, comentários e interações entre usuários**.

---

# Arquitetura do Projeto

O projeto foi estruturado utilizando uma arquitetura organizada em camadas para garantir **separação de responsabilidades, escalabilidade e manutenibilidade**.



SocialMedia
│
├── API → Camada de apresentação (Controllers / Endpoints)
├── Application → Regras de aplicação e serviços
├── Domain → Entidades e regras de negócio
├── Infrastructure → Acesso a dados e integrações externas
└── Structure → Configurações e organização geral


Essa separação permite manter o código limpo, testável e preparado para evolução futura.

---

# Funcionalidades

### Autenticação e Segurança
- Registro de novos usuários
- Login com autenticação via **JWT**
- Autorização baseada em token
- Senhas criptografadas utilizando **BCrypt**
- Proteção de rotas autenticadas

### Gerenciamento de Usuários
- Criação de usuário
- Autenticação e sessão
- Soft delete de contas (usuários não são removidos permanentemente)

### Sistema de Posts
- Criação de posts
- Exclusão de posts
- Sistema de curtidas e descurtidas
- Recuperação de posts

### Sistema de Comentários
- Criação de comentários em posts
- Exclusão de comentários

### Modelagem de Banco de Dados
Relacionamentos implementados:

- **One-to-Many**
- **Many-to-One**
- **Many-to-Many**

Entidades principais:

- User
- Post
- Comment
- Like
- Relationship

### Validação de Dados
- **Data Annotations**
- **Fluent API**
- Uso de **DTOs** para evitar exposição direta das entidades e garantir integridade dos dados.

### Versionamento de API
A API utiliza **versionamento de endpoints**, permitindo evolução da aplicação sem quebrar versões anteriores.

---

# Documentação e Testes da API

A API pode ser testada e explorada utilizando:

### Scalar
Ferramenta moderna de documentação e exploração da API.

### Postman
Utilizado para testes internos de endpoints e validação de fluxos de autenticação.

---

# Tecnologias Utilizadas

- **ASP.NET Core**
- **C#**
- **.NET**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **BCrypt**
- **Scalar**
- **Postman**
- **Git**
- **GitHub Copilot**

---

# Ferramentas de Desenvolvimento

- **VS Code** → desenvolvimento da aplicação  
- **GitHub Copilot** → produtividade e assistência de código  
- **Postman** → testes de endpoints  
- **Scalar** → documentação da API  

---

# Objetivo do Projeto

Este projeto foi desenvolvido com foco em **aprendizado e aprofundamento em desenvolvimento backend com .NET**, explorando conceitos importantes como:

- arquitetura em camadas
- autenticação segura
- modelagem de banco de dados
- boas práticas de API REST
- validação de dados
- organização de código escalável

---

# Melhorias Futuras

- Sistema de seguidores (followers)
- Feed personalizado
- Sistema de notificações
- Upload de imagens
- Paginação de posts
- Testes automatizados

---

# Autor

Gabriel Olímpio  
Backend Developer (.NET)

GitHub:
https://github.com/GabrielCAOlimpio