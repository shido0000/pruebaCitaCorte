# TODO - Implementación Sistema Multibarbero

## Backend - Fase 1: Enums y Entidades
- [x] API.Data/Enum/Multibarbero/TipoPlan.cs
- [x] API.Data/Enum/Multibarbero/TipoProveedor.cs
- [x] API.Data/Enum/Multibarbero/EstadoReserva.cs
- [x] API.Data/Enum/Multibarbero/EstadoSolicitudAfiliacion.cs
- [x] API.Data/Enum/Multibarbero/EstadoSuscripcion.cs
- [x] API.Data/Enum/Multibarbero/TipoNotificacion.cs
- [x] API.Data/Enum/DiaSemana.cs
- [x] API.Data/Entidades/Multibarbero/PlanSuscripcion.cs
- [x] API.Data/Entidades/Multibarbero/CaracteristicaPlan.cs
- [x] API.Data/Entidades/Multibarbero/LimitesPlan.cs
- [x] API.Data/Entidades/Multibarbero/PerfilBarbero.cs
- [x] API.Data/Entidades/Multibarbero/PerfilBarberia.cs
- [x] API.Data/Entidades/Multibarbero/Servicio.cs
- [x] API.Data/Entidades/Multibarbero/CategoriaServicio.cs
- [x] API.Data/Entidades/Multibarbero/PerfilCliente.cs
- [x] API.Data/Entidades/Multibarbero/Producto.cs
- [x] API.Data/Entidades/Multibarbero/CategoriaProducto.cs
- [x] API.Data/Entidades/Multibarbero/Reserva.cs
- [x] API.Data/Entidades/Multibarbero/SolicitudAfiliacion.cs
- [x] API.Data/Entidades/Multibarbero/AfiliacionBarbero.cs
- [x] API.Data/Entidades/Multibarbero/Notificacion.cs
- [x] API.Data/Entidades/Multibarbero/EstadisticaBarbero.cs
- [x] API.Data/Entidades/Multibarbero/EstadisticaBarberia.cs
- [x] API.Data/Entidades/Multibarbero/EstadisticaProducto.cs

## Backend - Fase 2: Configuraciones BD
- [x] API.Data/ConfiguracionEntidades/Multibarbero/PlanSuscripcionConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/PerfilBarberoConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/PerfilBarberiaConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/ReservaConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/SolicitudAfiliacionConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/AfiliacionBarberoConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/NotificacionConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/ServicioConfiguracionBD.cs
- [x] API.Data/ConfiguracionEntidades/Multibarbero/ProductoConfiguracionBD.cs

## Backend - Fase 3: Repositorios
- [x] API.Data/IUnitOfWorks/Interfaces/Multibarbero/IPlanSuscripcionRepository.cs
- [x] API.Data/IUnitOfWorks/Interfaces/Multibarbero/IPerfilBarberoRepository.cs
- [x] API.Data/IUnitOfWorks/Interfaces/Multibarbero/IReservaRepository.cs
- [x] API.Data/IUnitOfWorks/Interfaces/Multibarbero/ISolicitudAfiliacionRepository.cs
- [x] API.Data/IUnitOfWorks/Interfaces/Multibarbero/INotificacionRepository.cs
- [x] API.Data/IUnitOfWorks/Repositorios/Multibarbero/PlanSuscripcionRepository.cs
- [x] API.Data/IUnitOfWorks/Repositorios/Multibarbero/PerfilBarberoRepository.cs
- [x] API.Data/IUnitOfWorks/Repositorios/Multibarbero/ReservaRepository.cs
- [x] API.Data/IUnitOfWorks/Repositorios/Multibarbero/SolicitudAfiliacionRepository.cs
- [x] API.Data/IUnitOfWorks/Repositorios/Multibarbero/NotificacionRepository.cs

## Backend - Fase 4: Servicios Dominio
- [ ] API.Domain/Interfaces/Multibarbero/IPlanSuscripcionService.cs
- [ ] API.Domain/Interfaces/Multibarbero/IPerfilBarberoService.cs
- [ ] API.Domain/Interfaces/Multibarbero/IReservaService.cs
- [ ] API.Domain/Interfaces/Multibarbero/ISolicitudAfiliacionService.cs
- [ ] API.Domain/Interfaces/Multibarbero/INotificacionService.cs
- [ ] API.Domain/Services/Multibarbero/PlanSuscripcionService.cs
- [ ] API.Domain/Services/Multibarbero/PerfilBarberoService.cs
- [ ] API.Domain/Services/Multibarbero/ReservaService.cs
- [ ] API.Domain/Services/Multibarbero/SolicitudAfiliacionService.cs
- [ ] API.Domain/Services/Multibarbero/NotificacionService.cs

## Backend - Fase 5: DTOs
- [ ] PlanSuscripcion DTOs
- [ ] PerfilBarbero DTOs
- [ ] Reserva DTOs
- [ ] Notificacion DTOs
- [ ] Afiliacion DTOs

## Backend - Fase 6: Validadores
- [x] Validadores para cada entity
  - [x] CrearPlanSuscripcionDtoValidator.cs
  - [x] CrearPerfilBarberoDtoValidator.cs
  - [x] CrearReservaDtoValidator.cs
  - [x] CrearSolicitudAfiliacionDtoValidator.cs
  - [x] CrearNotificacionDtoValidator.cs

## Backend - Fase 7: Controllers
- [ ] Controladores API

## Backend - Fase 8: Actualización configs existentes
- [ ] Actualizar IApiDbContext.cs
- [ ] Actualizar ApiDbContext.cs
- [ ] Actualizar IUnitOfWork.cs
- [ ] Actualizar UnitOfWork.cs
- [ ] Actualizar IoCRegister.cs

## Frontend - Vue 3
- [ ] Crear proyecto Vue 3
- [ ] Implementar componentes y vistas
