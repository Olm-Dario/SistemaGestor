-- PROCEDIMIENTO ALMACENADO PARA EL REGISTRO DE CLIENTES --

create proc sp_RegistrarCliente(
@Documento varchar(50),
@Nombre varchar(50),
@Apellido varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
) as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT

	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento)
	begin
	INSERT INTO CLIENTE(Documento,Nombre,Apellido, Correo, Telefono, Estado) values(
	@Documento, @Nombre, @Apellido, @Correo, @Telefono, @Estado)

	set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documento ya existe'

end

go

-- PROCEDIMIENTO ALMACENADO PARA LA ACTUALIZACIÓN DE CLIENTE
CREATE PROC sp_ModificarCliente(
@IdCliente int,
@Documento varchar(50),
@Nombre varchar(50),
@Apellido varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento and IdCliente !=@IdCliente)
	begin
		update CLIENTE set
		Documento = @Documento,
		Nombre = @Nombre,
		Apellido = @Apellido,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE IdCliente = @IdCliente
	end

	else
	begin
		SET @Resultado = 0
		SET @Mensaje = 'El numero de documento ya existe'
	end
end
