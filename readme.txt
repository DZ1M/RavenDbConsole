##Colocar RavenDB rodar em um Docker
docker run -d -p 8080:8080 -e RAVEN_ARGS="--Setup.Mode=None --License.Eula.Accepted=true" ravendb/ravendb

## Instar Pacote
dotnet add package RavenDb.Client