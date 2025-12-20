-- 1. Önce 'admin' rolü var mı kontrol et, yoksa oluştur
IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (NEWID(), 'admin', 'ADMIN');
END

-- 2. Senin kullanıcını bul ve Admin rolünü ver
DECLARE @UserId nvarchar(450)
DECLARE @RoleId nvarchar(450)
DECLARE @UserEmail nvarchar(256)

-- BURAYA KENDİ MAİL ADRESİNİ YAZ
SET @UserEmail = 'g231210049@sakarya.edu.tr' 

SELECT @UserId = Id FROM AspNetUsers WHERE Email = @UserEmail
SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'admin'

IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT * FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)
        PRINT 'TEBRIKLER! Kullanici basariyla ADMIN yapildi.'
    END
    ELSE
    BEGIN
        PRINT 'Bu kullanici zaten admin.'
    END
END
ELSE
BEGIN
    PRINT 'HATA: Kullanici bulunamadi. Lutfen once site uzerinden kayit olun.'
END
