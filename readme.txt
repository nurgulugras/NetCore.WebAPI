Host: xxxx

dotnet ef migrations add initial --project ../ALMS.Data/ALMS.Data.csproj
dotnet ef database update --project ../ALMS.Data/ALMS.Data.csproj


Servis kullanımda olduğu durumda ilgili servisi kill etme :
lsof -i :6007
kill -9 

-- nuget cache clear
dotnet nuget locals all --clear


**** (Mac) EF DB Operasyonları için ortam belirleme :
export ASPNETCORE_ENVIRONMENT=Devlopment
echo $ASPNETCORE_ENVIRONMENT
