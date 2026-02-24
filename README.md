# PicPay Simplificado

Uma implementação simplificada do desafio backend do PicPay - uma plataforma de pagamentos que permite transferências entre usuários e lojistas.

## 📋 Sobre o Projeto

Este projeto implementa uma API RESTful para um sistema de pagamentos simplificado, onde:

- **Usuários comuns** podem enviar e receber dinheiro
- **Lojistas** apenas recebem transferências (não podem enviar)
- Cada usuário possui uma **carteira (wallet)** com saldo
- Transferências são registradas no sistema

## 🛠️ Tecnologias

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL 16** - Banco de dados
- **Docker** - Containerização do banco de dados
- **OpenAPI** - Documentação da API

## 📁 Estrutura do Projeto

```
picpay-simple/
├── Controllers/
│   ├── WalletController.cs      # Endpoints de carteira
│   └── TransferController.cs    # Endpoints de transferência
├── DTOs/
│   ├── WalletDTOs.cs            # DTOs de carteira
│   └── TransferDTOs.cs          # DTOs de transferência
├── Services/
│   ├── IWalletService.cs        # Interface do serviço de carteira
│   ├── WalletService.cs         # Implementação do serviço de carteira
│   ├── ITransferService.cs      # Interface do serviço de transferência
│   └── TransferService.cs       # Implementação do serviço de transferência
├── docker/
│   └── docker-compose.yml       # Configuração do PostgreSQL
├── infra/
│   ├── AppDbContext.cs          # Contexto do EF Core
│   └── Config/
│       ├── TransferConfig.cs    # Configuração da entidade Transfer
│       └── WalletConfig.cs      # Configuração da entidade Wallet
├── Migrations/                   # Migrações do banco de dados
├── models/
│   ├── enums/
│   │   └── UserType.cs          # Enum: user, merchant
│   ├── TransferEntity.cs        # Entidade de transferência
│   └── WalletEntity.cs          # Entidade de carteira/usuário
├── Program.cs                    # Ponto de entrada da aplicação
├── appsettings.json             # Configurações da aplicação
└── picpay-simple.csproj         # Arquivo de projeto
```

## 🚀 Como Executar

### Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)

### 1. Iniciar o Banco de Dados

```bash
cd docker
docker-compose up -d
```

Isso iniciará um container PostgreSQL com:
- **Host:** localhost
- **Porta:** 5432
- **Database:** piscapy_db
- **Usuário:** piscapy
- **Senha:** piscapy123

### 2. Aplicar Migrações

```bash
dotnet ef database update
```

### 3. Executar a Aplicação

```bash
dotnet run
```

A API estará disponível em `http://localhost:5037`

## 📡 Endpoints da API

### Carteiras (Wallets)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/wallet` | Criar nova carteira |
| `GET` | `/api/wallet` | Listar todas as carteiras |
| `GET` | `/api/wallet/{id}` | Obter carteira por ID |
| `POST` | `/api/wallet/{id}/deposit` | Depositar na carteira |
| `DELETE` | `/api/wallet/{id}` | Remover carteira |

### Transferências (Transfers)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/transfer` | Realizar transferência |
| `GET` | `/api/transfer` | Listar todas as transferências |
| `GET` | `/api/transfer/{id}` | Obter transferência por ID |
| `GET` | `/api/transfer/wallet/{walletId}` | Listar transferências de uma carteira |

## 📖 Exemplos de Uso

### Criar uma carteira (usuário comum)

```bash
curl -X POST http://localhost:5037/api/wallet \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "cpfcnpj": "12345678901",
    "email": "joao@email.com",
    "password": "senha123",
    "userType": 0
  }'
```

### Criar uma carteira (lojista)

```bash
curl -X POST http://localhost:5037/api/wallet \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Loja ABC",
    "cpfcnpj": "12345678000190",
    "email": "loja@email.com",
    "password": "senha123",
    "userType": 1
  }'
```

### Depositar na carteira

```bash
curl -X POST http://localhost:5037/api/wallet/{id}/deposit \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.00
  }'
```

### Realizar uma transferência

```bash
curl -X POST http://localhost:5037/api/transfer \
  -H "Content-Type: application/json" \
  -d '{
    "senderId": "uuid-do-remetente",
    "receiverId": "uuid-do-destinatario",
    "amount": 50.00
  }'
```

## 📊 Modelo de Dados

### Wallet (Carteira)

| Campo        | Tipo     | Descrição                          |
|--------------|----------|------------------------------------|
| Id           | UUID     | Identificador único                |
| Name         | string   | Nome do usuário                    |
| CPFCNPJ      | string   | CPF ou CNPJ (único)               |
| Email        | string   | Email (único)                      |
| PasswordHash | string   | Hash da senha                      |
| Balance      | decimal  | Saldo da carteira                  |
| UserType     | enum     | Tipo: `user` (0) ou `merchant` (1) |

### Transfer (Transferência)

| Campo        | Tipo     | Descrição                          |
|--------------|----------|------------------------------------|
| TransferId   | UUID     | Identificador único                |
| SenderId     | UUID     | ID do remetente                    |
| ReceiverId   | UUID     | ID do destinatário                 |
| Amount       | decimal  | Valor da transferência             |
| TransferDate | DateTime | Data/hora da transferência         |

## 🔧 Configuração

A connection string do banco de dados está em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=piscapy_db;Username=piscapy;Password=piscapy123"
  }
}
```

## 📝 Regras de Negócio

1. **Usuários comuns** (`user`) podem enviar e receber transferências
2. **Lojistas** (`merchant`) só podem receber transferências
3. CPF/CNPJ e Email devem ser únicos no sistema
4. Transferências devem ter valor positivo
5. O remetente deve ter saldo suficiente para a transferência
6. O remetente e destinatário não podem ser iguais

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT.
