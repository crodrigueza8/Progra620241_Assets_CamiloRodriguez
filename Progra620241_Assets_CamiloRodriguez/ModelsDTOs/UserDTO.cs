namespace Progra620241_Assets_CamiloRodriguez.ModelsDTOs
{
    public class UserDTO
    {

        //Un DTO Dat Transfer Object  Sirve para varios objetivos:

        //Desacoplar el modelo de la funcionalidad de los controladores para evitar que en futuras
        //actualizaciones de los modelospuedan ocurrir errores dificiles de reparar.

        //Sirve para simplificar modelos muy complejos y que tiene composiciones recursivas,
        //muy comunes cuando se generan por ORM como entity Framework, Dapper, Django...

        //Por un asunto de seguridad ya que normalmente los equipos de desarrollo de las apps y web APIs
        //estan separados y no se quiere que los programadores de frontend sepan como esta estructurada
        //la base de datos tomando como base los modelos. */

        public int CodigoUsuario { get; set; }

        public string Cedula { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string Correo { get; set; } = null!;

        /*En este Ejemplo no usaremos la contrase;a 
         ya que este DTO sera usado para mostrar la lista de usuarios en una UI
        tendremos otra version de DTO qui si tiene la contrase;ia para cuando querramos 
        agregar un usuario*/
        //public string Contrasennia { get; set; } = null!;

        public bool? Activo { get; set; }

        public int CodigoDeRol { get; set; }

        public string? RolDeUsuario { get; set; }

        public string? NotasDelUsuario { get; set; }

        //Aca se pueden agregar los atributos que sean necesarios

    }
}
