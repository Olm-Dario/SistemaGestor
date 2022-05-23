create proc SP_REGISTRARUSURARIO(
@Documento varchar(50),
@Nombre varchar(100),
@Apellido varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''

	if not exists(select * from USUARIO where Documento = @Documento)
	begin
		insert into usuario(Documento,Nombre,Apellido,Correo,Clave,IdRol,Estado) values
		(@Documento,@Nombre,@Apellido,@Correo,@Clave,@IdRol,@Estado)
		
		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'No se puede repetir el documento para más de un usuario'
end

go
/*editar*/
create proc SP_EDITARUSUARIO(
@IdUsuario int,
@Documento varchar(50),
@Nombre varchar(100),
@Apellido varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''

	if not exists(select * from USUARIO where Documento = @Documento and idusuario != @IdUsuario)
	begin
		update usuario set
		Documento = @Documento,
		Nombre = @Nombre,
		Apellido = @Apellido,
		Correo = @Correo,
		Clave = @Clave,
		IdRol = @IdRol,
		Estado = @Estado
		where IdUsuario = @IdUsuario
		
		set @Respuesta = 1
	end
	else
		set @Mensaje = 'No se puede repetir el documento para más de un usuario'
end


go
/*eliminar*/
create proc SP_ELIMINARUSUARIO(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	if exists (select * from COMPRA c
	inner join USUARIO u on u.IdUsuario = c.IdUsuario
	where u.IdUsuario = @IdUsuario)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una COMPRA\n'
	end

	if exists (select * from VENTA v
	inner join USUARIO u on u.IdUsuario = v.IdUsuario
	where u.IdUsuario = @IdUsuario)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una VENTA\n'
	end

	if(@pasoreglas = 1)
	begin
		delete from USUARIO where IdUsuario = @IdUsuario
		set @Respuesta = 1
	end

end
