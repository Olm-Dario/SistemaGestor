-- PROCEDIMIENTO ALMACENADO PARA EL REGISTRO DE PROVEEDORS --

create proc sp_RegistrarProveedor(
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
) as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT

	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento)
	begin
	INSERT INTO PROVEEDOR(Documento,RazonSocial, Correo, Telefono, Estado) values(
	@Documento, @RazonSocial, @Correo, @Telefono, @Estado)

	set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documento ya existe'

end

go

-- PROCEDIMIENTO ALMACENADO PARA LA ACTUALIZACIÓN DE PROVEEDOR
CREATE PROC sp_ModificarProveedor(
@IdPROVEEDOR int,
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento and IdPROVEEDOR !=@IdPROVEEDOR)
	begin
		update PROVEEDOR set
		Documento = @Documento,
		RazonSocial = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE IdPROVEEDOR = @IdPROVEEDOR
	end

	else
	begin
		SET @Resultado = 0
		SET @Mensaje = 'El numero de documento ya existe'
	end
end

go

create procedure sp_EliminarProveedor(
@IdProveedor int,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS(
	SELECT * FROM PROVEEDOR p
	inner join COMPRA c on p.IdProveedor = c.IdProveedor
	where p.IdProveedor = @IdProveedor
	)

	begin
		DELETE TOP(1) FROM PROVEEDOR WHERE IdProveedor = @IdProveedor
	end

	ELSE

	begin
		SET @Resultado = 0
		SET @Mensaje = 'El proveedor se encuentra relacionado a una compra'
	end

end