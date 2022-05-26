/*----------- PROCEDIMIENTOS PARA PRODUCTO ------------*/

/*REGISTRAR*/
create proc SP_RegistrarProducto (
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar(30),
@IdCategoria int,
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
) as
begin
	set @Resultado = 0
	if not exists (select * from PRODUCTO where Codigo = @Codigo)
	begin
		insert into PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
		values (@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)
		
		set @Resultado = SCOPE_IDENTITY();
	end
	else
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
end


go 

/*EDITAR*/
create proc SP_ModificarProducto (
@IdProducto int,
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar(30),
@IdCategoria int,
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
) as
begin
	set @Resultado = 1
	if not exists (select * from PRODUCTO where Codigo = @Codigo and IdProducto != @IdProducto)
	begin
		update PRODUCTO set
		Codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdCategoria = @IdCategoria,
		Estado = @Estado
		where IdProducto = @IdProducto
	
	end
	else
		set @Resultado = 0
		set @Mensaje = 'Ya existe un producto con el mismo codigo'
end


go



/*ELIMINAR*/
create proc SP_EliminarProducto (
@IdProducto int,
@Respuesta bit output,
@Mensaje varchar(500) output
) as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	if exists (select * from DETALLE_COMPRA dc
	inner join PRODUCTO p on p.IdProducto = dc.IdProducto
	where p.IdProducto = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra ralacionada a una COMPRA\n'
	end
	 
	if exists (select * from DETALLE_VENTA dv
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	where p.IdProducto = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra ralacionada a una VENTA\n'
	end

	if(@pasoreglas = 1)
	begin
		delete from PRODUCTO where IdProducto = @IdProducto
		set @Respuesta = 1
	end
end