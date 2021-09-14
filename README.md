# NetCore-HangFire
Este projeto tem o intuito de demonstrar o uso do HangFire para o agendamento de mensagens que irão rodar em background

Para rodar o projeto é necessário criar um banco no SQL Server de forma que o Hangfire utilize-o para criar a estrutura de tabela necessária, para isso rodar o script a seguir:

####################################################################

CREATE LOGIN [HangFireUser] WITH PASSWORD = 'Us3rP@ssw0rd'

GO

CREATE DATABASE HangFireDB

GO

use [HangFireDB]

CREATE USER [HangFireUser] 

FOR LOGIN [HangFireUser]

WITH DEFAULT_SCHEMA = dbo; 

GO  
 
EXEC sp_change_users_login 'Update_One', 'HangFireUser', 'HangFireUser';

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE [name] = 'HangFire') EXEC ('CREATE SCHEMA [HangFire]')

GO

ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [HangFireUser]

GO

GRANT CREATE TABLE TO [HangFireUser]

GO

####################################################################


Após rodar o script, o projeto está apto a ser restaurado e rodar para agendar as tarefas através dos endpoints disponíbilizados através da api.

link de teste da api pelo swagger

https://localhost:5003/swagger/index.html

link da dashboard do Hangfire

https://localhost:5003/hangfire
