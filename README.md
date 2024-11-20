# CRC-API
## Ideia
O projeto CRC-API visa criar uma aplicação que ajude condomínios a incentivar seus moradores a reduzir o consumo de energia elétrica por meio de um sistema de recompensas (bônus). Através da implementação de métricas e incentivos, o sistema permitirá que os moradores acompanhem o consumo de energia e sejam premiados por atingirem metas de redução, promovendo assim a sustentabilidade e o uso responsável dos recursos.

## Objetivo
O objetivo principal da solução é fornecer uma plataforma que permita o gerenciamento completo dos dados e das operações de redução de consumo de energia nos condomínios. A aplicação gerencia dados dos moradores, consumo de energia, bonificação e a interação entre essas entidades, garantindo a eficiência e transparência do sistema.

## Como rodar
- Coloque as credencias do banco de dados Oracle no arquivo appsettings.json
- Execute update database no terminal de gerenciamento de pacotes do NuGet
- Inicie a aplicação em http

## Funcionalidades
- Gerenciamento de Condomínios: Cadastro e gerenciamento de informações dos condomínios, incluindo dados como nome, endereço e status.
- Gerenciamento de Moradores: Cadastro e controle das informações dos moradores, com a capacidade de associá-los a um condomínio específico.
- Monitoramento de Consumo: Acompanhamento do consumo de energia de cada morador e comparações com o consumo histórico.
- Sistema de Bônus: Definição de regras e critérios para o cálculo e distribuição de bônus aos moradores com base na redução do consumo de energia.

## Entidades
- Auth: Para autenticação
- Condominio: Para gerenciamento dos Condominios
- Morador: Para gerenciamento dos moradores dos condominios
- Bonus: Para gerenciamento dos bonus dos condominios
- Fatura: Para gerenciamento do consumo de energia dos moradores
- MoradorBonus: Para gerenciamento da relação entre as entidades Morador e Bonus
