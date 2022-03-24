cd /Users/tanerselek/Development/Projects/Productions/ElsaBilisim/Projects/Products/AppLicenseManagementSystem/Development/NetCore.WebAPI/ALMS.WebAPI
rm -rf bin/Release/netcoreapp3.1/linux-x64
dotnet publish -c Release -r linux-x64

if [ -d "bin/Release/netcoreapp3.1/linux-x64/publish/" ] 
then

    cd bin/Release/netcoreapp3.1/linux-x64/publish/

    echo "zip ile sıkıştırılıyor..."
    tar -zcvf publish.tar.gz *

    echo "zip transfer ediliyor..."
    sshpass -p "123456aA" scp -r publish.tar.gz root@192.168.100.11:/var/www/services/prod/zip

    echo "publish dataları siliniyor..."
    sshpass -p "123456aA" ssh root@192.168.100.11 'echo 123456aA | sudo -S rm -rf /var/www/services/prod/alms/*'

    echo "zipten çıkartılıyor..."
    sshpass -p "123456aA" ssh root@192.168.100.11 'echo 123456aA | sudo -S tar -xvf /var/www/services/prod/zip/publish.tar.gz -C /var/www/services/prod/alms/'

    echo "aktarılan zip siliniyor..."
    sshpass -p "123456aA" ssh root@192.168.100.11 'echo 123456aA | sudo -S rm -rf /var/www/services/prod/zip/publish.tar.gz'

    echo "servis yeniden başlatılıyor"
    sshpass -p "123456aA" ssh root@192.168.100.11 'echo 123456aA | sudo -S systemctl restart webapi-kestrel-alms-prod.service; systemctl status webapi-kestrel-alms-prod.service; exit'
    sshpass -p "123456aA" ssh root@192.168.100.11 'echo 123456aA | sudo -S nginx -s reload; exit'

    echo işlem tamam

else
    echo "build üretilemedi!"
fi