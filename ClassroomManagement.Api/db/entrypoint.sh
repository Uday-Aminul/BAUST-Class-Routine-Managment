#!/bin/bash
/opt/mssql/bin/sqlservr &

SQLCMD="/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P $MSSQL_SA_PASSWORD -b"

# wait until SQL Server accepts connections
for i in {1..60}; do
  $SQLCMD -Q "SELECT 1" > /dev/null 2>&1 && break
  sleep 2
done

$SQLCMD -Q "IF DB_ID('ClassroomDb') IS NULL CREATE DATABASE ClassroomDb"
$SQLCMD -d ClassroomDb -i schema.sql

wait