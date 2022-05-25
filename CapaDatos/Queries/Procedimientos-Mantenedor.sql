--PROCEDIMIENTO PARA GUARDAR CATEGORIA
create proc SP_RegistrarCategoria(
@Descripcion varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output) as
begin
	set @Resultado = 0
	if not exists (select * from CATEGORIA where Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion,Estado) values (@Descripcion,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
end

go

--PROCEDIMIENTO PARA MODIFICAR CATEGORIA
create proc SP_EditarCategoria(
@IdCategoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output) as
begin
	set @Resultado = 1
	if not exists (select * from CATEGORIA where Descripcion = @Descripcion and IdCategoria != @IdCategoria)
		update CATEGORIA set
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	else
		begin
			set @Resultado = 0
			set @Mensaje = 'No se puede repetir la descripcion de una categoria' 
		end
end

go


--PROCEDIMIENTO PARA MODIFICAR CATEGORIA
create proc SP_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje varchar(500) output) as
begin
	set @Resultado = 1
	if not exists (
	select * from CATEGORIA c
	inner join PRODUCTO p on p.IdCategoria = c.IdCategoria
	where c.IdCategoria != @IdCategoria)
		begin
			delete top(1) from CATEGORIA 
			where IdCategoria = @IdCategoria
		end
	else
		begin
			set @Resultado = 0
			set @Mensaje = 'La categoria se encuentra relacionada a un producto' 
		end
end

