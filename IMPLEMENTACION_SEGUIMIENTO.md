# SEGUIMIENTO DE IMPLEMENTACIÓN - SISTEMA MULTIBARBERO

## ESTADO GENERAL: EN PROGRESO (40% completado)

---

## ✅ IMPLEMENTADO

### Entidades del Dominio
- [x] PlanSuscripcion (básico)
- [x] CaracteristicaPlan
- [x] LimitesPlan (estructura básica - requiere actualización)
- [x] PerfilBarbero (parcial - faltan campos críticos)
- [x] PerfilBarberia (parcial - faltan campos críticos)
- [x] PerfilCliente
- [x] Servicio
- [x] CategoriaServicio
- [x] Producto
- [x] CategoriaProducto
- [x] Reserva
- [x] SolicitudAfiliacion
- [x] AfiliacionBarbero
- [x] Notificacion
- [x] EstadisticaBarbero
- [x] EstadisticaBarberia
- [x] EstadisticaProducto

### Enums
- [x] TipoPlan
- [x] EstadoSuscripcion
- [x] EstadoSolicitudAfiliacion
- [x] TipoProveedor
- [x] EstadoReserva
- [x] TipoNotificacion

### Servicios
- [x] PerfilBarberiaService (básico)
- [x] ServicioService (básico)
- [ ] ProductoService (pendiente)
- [ ] EstadisticaService (pendiente)
- [ ] SuscripcionService (pendiente)
- [ ] AfiliacionService (pendiente)
- [ ] NotificacionService (pendiente)

### Controladores
- [x] CategoriaProductoController
- [x] CategoriaServicioController
- [x] EstadisticaController (básico)
- [ ] PerfilBarberiaController (pendiente)
- [ ] ServicioController (pendiente)
- [ ] ProductoController (pendiente)
- [ ] SuscripcionController (pendiente)
- [ ] AfiliacionController (pendiente)
- [ ] NotificacionController (pendiente)

### DTOs
- [x] DTOs básicos para entities existentes
- [ ] DTOs completos con patrón CRUD (pendiente)
- [ ] DTOs de filtrado y paginación avanzados (pendiente)

### Validadores
- [x] CrearPerfilBarberoDtoValidator
- [x] CrearServicioInputDtoValidator
- [x] CrearReservaDtoValidator
- [x] CrearSolicitudAfiliacionDtoValidator
- [x] CrearPlanSuscripcionDtoValidator
- [ ] Validadores de actualización (pendientes)
- [ ] Validadores específicos de reglas de negocio (pendientes)

---

## ❌ FALTANTE CRÍTICO

### 1. Actualización de Entidades

#### PerfilBarbero.cs - Campos Faltantes
- [ ] PlanSuscripcionId (FK)
- [ ] FechaInicioPlan (DateTime?)
- [ ] FechaVencimientoPlan (DateTime?)
- [ ] EstadoSolicitudCambioPlan (Enum: Pendiente/Aprobado/Rechazado)
- [ ] NuevoPlanSolicitadoId (Guid?)
- [ ] TotalServicios (int)
- [ ] TotalClientes (int)

#### PerfilBarberia.cs - Campos Faltantes
- [ ] NombreComercial (string)
- [ ] Latitud (decimal?)
- [ ] Longitud (decimal?)
- [ ] HorarioApertura (TimeSpan?)
- [ ] HorarioCierre (TimeSpan?)
- [ ] DiasLaborablesJson (string - JSON array)
- [ ] CapacidadMaxima (int?)
- [ ] MaxBarberosPermitidos (int)
- [ ] EstadoSolicitudCambioPlan (Enum)
- [ ] NuevoPlanSolicitadoId (Guid?)
- [ ] FechaInicioPlan (DateTime?)
- [ ] FechaVencimientoPlan (DateTime?)

#### LimitesPlan.cs - Reestructuración Completa
- [ ] Eliminar estructura actual (TipoLimite/Valor)
- [ ] MaxBarberosAfiliados (int)
- [ ] MaxReservasMensuales (int)
- [ ] MaxProductosVenta (int)
- [ ] PermiteEstadisticas (bool)
- [ ] PermiteReservas (bool)
- [ ] PermiteProductos (bool)

### 2. Sistema de Validación de Solapamiento de Reservas
- [ ] Crear servicio de validación de horarios
- [ ] Implementar método HaySolapamiento()
- [ ] Integrar en creación/actualización de reservas
- [ ] Considerar tiempo de preparación entre servicios
- [ ] Bloquear horarios no disponibles

### 3. Gestión de Suscripciones
- [ ] Flujo de solicitud de cambio de plan
- [ ] Aprobación/rechazo por Admin/Comercial
- [ ] Estados de suscripción (Pendiente, Activo, Vencido, Cancelado)
- [ ] Notificaciones automáticas de vencimiento
- [ ] Cálculo automático de fechas de vencimiento

### 4. Sistema de Afiliación
- [ ] Validación de cupo máximo por barbería
- [ ] Control de afiliaciones únicas (una activa por barbero)
- [ ] Redirección automática de reservas a barbería
- [ ] Historial de afiliaciones
- [ ] Búsqueda de barberos afiliados

### 5. Servicios Faltantes
- [ ] ProductoService (CRUD completo)
- [ ] EstadisticaService (cálculo y consulta)
- [ ] SuscripcionService (gestión de planes)
- [ ] AfiliacionService (gestión de afiliaciones)
- [ ] NotificacionService (envío y gestión)
- [ ] ReservaService (con validación de solapamiento)

### 6. Controladores Faltantes
- [ ] PerfilBarberiaController (CRUD completo)
- [ ] ServicioController (CRUD completo)
- [ ] ProductoController (CRUD completo)
- [ ] SuscripcionController (gestión de planes)
- [ ] AfiliacionController (gestión de solicitudes)
- [ ] NotificacionController (lectura/marcado)

### 7. DTOs Faltantes
- [ ] Crear/Actualizar PerfilBarberia DTOs
- [ ] Crear/Actualizar Servicio DTOs completos
- [ ] Crear/Actualizar Producto DTOs
- [ ] DTOs para gestión de suscripciones
- [ ] DTOs para gestión de afiliaciones
- [ ] DTOs de estadísticas detalladas

### 8. Configuración Técnica
- [ ] Configuración completa de DbContext
- [ ] Implementación de UnitOfWork completa
- [ ] Registro de dependencias en IoC
- [ ] Seeders de roles y planes iniciales
- [ ] Configuración de Hangfire para jobs programados

---

## 📋 PLAN DE IMPLEMENTACIÓN

### FASE 1: Entidades (Prioridad ALTA)
1. Actualizar PerfilBarbero con campos de suscripción
2. Actualizar PerfilBarberia con campos completos
3. Reestructurar LimitesPlan
4. Agregar enums faltantes si son necesarios

### FASE 2: Reglas de Negocio (Prioridad ALTA)
1. Implementar validación de solapamiento de reservas
2. Crear flujo de gestión de suscripciones
3. Implementar sistema de afiliación con validaciones

### FASE 3: Servicios (Prioridad MEDIA)
1. Crear servicios faltantes
2. Implementar lógica de negocio en servicios
3. Integrar notificaciones automáticas

### FASE 4: Controladores y DTOs (Prioridad MEDIA)
1. Crear controladores REST completos
2. Implementar DTOs con validaciones
3. Documentar endpoints con Swagger

### FASE 5: Configuración y Testing (Prioridad BAJA)
1. Configurar IoC y dependencias
2. Crear seeders de datos iniciales
3. Implementar tests unitarios

---

## 📝 NOTAS DE IMPLEMENTACIÓN

### Reglas de Negocio Críticas
1. **Solapamiento de Reservas**: No permitir reservas con horarios superpuestos para el mismo proveedor
2. **Límite de Barberos**: Barberías no pueden exceder límite de su plan
3. **Afiliación Única**: Barbero solo puede tener UNA afiliación activa principal
4. **Aprobación Requerida**: Cambios de plan y nuevos registros requieren aprobación
5. **Redirección de Reservas**: Si barbero está afiliado, reservas van a barbería

### Consideraciones Técnicas
- Usar transacciones para operaciones críticas
- Implementar patrón Repository/UnitOfWork correctamente
- Validar permisos por rol en controladores
- Logs adecuados para auditoría
- Manejo consistente de errores

---

Fecha de última actualización: 2025-05-03
Responsable: Asistente de Implementación
