# FinTrack - Gerenciador de Finanças Pessoais

> Uma API robusta para gerenciar finanças pessoais, permitindo controlar receitas, despesas e acompanhar o saldo em tempo real.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=.net)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat-square&logo=c-sharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=flat-square&logo=microsoft-sql-server)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

## 📋 Sobre o Projeto

FinTrack é uma aplicação backend construída em ASP.NET Core que oferece uma API RESTful completa para gerenciamento de finanças pessoais. O projeto permite que os usuários registrem suas transações (receitas e despesas) e acompanhem seu saldo em tempo real.

### ✨ Funcionalidades Principais

- **👤 Gerenciamento de Usuários**
  - Criar novo usuário
  - Visualizar perfil do usuário
  - Atualizar informações do usuário
  - Senha criptografada com BCrypt

- **💳 Gerenciamento de Transações**
  - Registrar receitas e despesas
  - Listar todas as transações do usuário
  - Visualizar detalhes de uma transação
  - Atualizar transações existentes
  - Deletar transações

- **💰 Cálculo de Saldo**
  - Cálculo automático do saldo (Receitas - Despesas)
  - Visualizar saldo atual do usuário
  - Atualização em tempo real

## 🏗️ Arquitetura do Projeto

O projeto segue a arquitetura em camadas:

```
FinTrack/
├── FinTrack.API/              # Camada de Apresentação (Controllers, DTOs)
│   ├── Controllers/            # Endpoints da API
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Services/               # Lógica de negócio
│   ├── Middlewares/            # Middleware de tratamento de exceções
│   └── Program.cs              # Configuração da aplicação
│
├── FinTrack.Domain/            # Camada de Domínio (Entidades, Interfaces)
│   ├── Entities/               # Classes de domínio (User, Transaction, Balance)
│   └── Interfaces/             # Contratos de repositórios e serviços
│
└── FinTrack.Infrastructure/    # Camada de Infraestrutura (Dados, Segurança)
    ├── Data/                   # DbContext e configurações
    ├── Repositories/           # Implementação de repositórios
    ├── Migrations/             # Migrações do Entity Framework
    └── Security/               # Implementação de criptografia
```

### 📦 Padrões Utilizados

- **Repository Pattern**: Abstração de acesso a dados
- **Dependency Injection**: Gerenciamento de dependências via DI Container
- **DTO (Data Transfer Object)**: Transferência segura de dados
- **Service Layer**: Lógica de negócio centralizada
- **Entity Framework Core**: ORM para persistência de dados

## 🛠️ Tecnologias Utilizadas

- **Framework**: .NET 10.0
- **Linguagem**: C# 12
- **Banco de Dados**: SQL Server
- **ORM**: Entity Framework Core 10.0.7
- **Hash de Senha**: BCrypt
- **API Documentation**: Swagger/OpenAPI
- **Serialização**: JSON

## 📋 Pré-requisitos

- .NET 10.0 SDK instalado
- SQL Server 2019 ou superior
- Visual Studio 2022 / VS Code
- PowerShell ou similar para comandos CLI

## 🚀 Configuração e Instalação

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/FinTrack.git
cd FinTrack
```

### 2. Configurar a conexão com o banco de dados

Edite o arquivo `FinTrack.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seu_servidor;Database=FinTrackDb;Trusted_Connection=true;Encrypt=false;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3. Restaurar dependências

```bash
dotnet restore
```

### 4. Aplicar migrações do banco de dados

```bash
dotnet ef database update --project FinTrack.Infrastructure --startup-project FinTrack.API
```

### 5. Executar a aplicação

```bash
dotnet run --project FinTrack.API
```

A API estará disponível em `https://localhost:7000` (ou a porta configurada em launchSettings.json)

## 📚 Documentação da API

Após iniciar a aplicação, acesse o Swagger em:

```
https://localhost:7000/swagger
```

### Endpoints Principais

#### 👤 Usuários
- `POST /api/users` - Criar novo usuário
- `GET /api/users/{id}` - Obter dados do usuário
- `PUT /api/users/{id}` - Atualizar usuário
- `DELETE /api/users/{id}` - Deletar usuário

#### 💳 Transações
- `POST /api/transactions` - Criar nova transação
- `GET /api/transactions` - Listar transações do usuário
- `GET /api/transactions/{id}` - Obter detalhes da transação
- `PUT /api/transactions/{id}` - Atualizar transação
- `DELETE /api/transactions/{id}` - Deletar transação

#### 💰 Saldo
- `GET /api/balance/{userId}` - Obter saldo do usuário
- `PUT /api/balance/{userId}` - Atualizar saldo

## 📊 Estrutura de Dados

### User (Usuário)
```csharp
- Id (Guid)
- Name (string)
- Email (string)
- PasswordHash (string)
- CreatedAt (DateTime)
- UpdatedAt (DateTime?)
- IsActive (bool)
```

### Transaction (Transação)
```csharp
- Id (Guid)
- UserId (Guid)
- Type (enum: Income/Expense)
- Amount (decimal)
- Description (string)
- Date (DateTime)
- CreatedAt (DateTime)
- UpdatedAt (DateTime?)
```

### Balance (Saldo)
```csharp
- Id (Guid)
- UserId (Guid)
- Balance (decimal)
- UpdatedAt (DateTime)
```

## 🔐 Segurança

- ✅ Senhas criptografadas com BCrypt
- ✅ Validação de entrada em todos os endpoints
- ✅ Tratamento centralizado de exceções
- ✅ Isolamento de dados por usuário

## 📈 Roadmap Futuro

- [ ] **Autenticação JWT**: Implementar login com geração de tokens
- [ ] **Autorização**: Adicionar [Authorize] para endpoints sensíveis
- [ ] **Modificação de Senha**: Permitir alteração segura de senha
- [ ] **Categorias de Transações**: Categorizar receitas e despesas
- [ ] **Relatórios**: Gerar relatórios de gastos por período
- [ ] **Validações Avançadas**: Validações mais robustas em todas as operações

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo LICENSE para detalhes.

## 👨‍💻 Autor

Seu Nome - [@seu_usuario](https://github.com/seu_usuario)

---

**Nota**: Este projeto está em desenvolvimento ativo. Verifique regularmente atualizações e novas funcionalidades!
