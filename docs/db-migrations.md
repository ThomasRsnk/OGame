Db Migrations
=============


Add Migration
-------------

```ps
Add-Migration -Project Djm.OGame.Web.Api.Dal -StartupProject Djm.OGame.Web.Api -Debug -Verbose <MigrationName>Migration
```

Update Database
---------------

```ps
Update-Database -Project Djm.OGame.Web.Api.Dal -StartupProject Djm.OGame.Web.Api -Debug -Verbose
```