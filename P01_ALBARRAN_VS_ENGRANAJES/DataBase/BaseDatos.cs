using Microsoft.Data.Sqlite;
using P01_ALBARRAN_VS_ENGRANAJES.RESOURCES;
using ScottPlot.Colormaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using P01_ALBARRAN_VS_ENGRANAJES.ViewModel;

namespace P01_ALBARRAN_VS_ENGRANAJES.DataBase
{
    internal sealed class BaseDatos
    {
        private readonly string _cadenaconexion;
        public BaseDatos()
        {
            string rutaDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "DATABASE_GEAR_VALUE.db");
            _cadenaconexion = $"Data Source={rutaDB};";
        }

        #region  Etracción de datos de módulos métricos
        public List<double> ObtenerModulo()
        {

            List<double> ModuloMetricoData = new List<double>();
            using (var conexion = new SqliteConnection(_cadenaconexion))
            {
                using (var comando = new SqliteCommand($"SELECT modulo_mm FROM ModulosMetricos", conexion))
                {

                    try
                    {
                        conexion.Open();
                        using (var miReader = comando.ExecuteReader())
                        {
                            while (miReader.Read())
                            {
                                double valor = Convert.ToSingle(miReader["modulo_mm"]);
                                ModuloMetricoData.Add(valor);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener datos: " + ex);
                    }
                    return ModuloMetricoData;
                }
            }

        }

        #endregion

        #region  Obtención de los factores J de base de datos
        public (double J_pinon, double J_corona) ObtenerFactoresJ(int Np, int Ng, int anguloPresion)
        {
            // .........         Variables utilizadas para extracción de datos dado un caso.


            // VARIABES PARA EXTRAER LOS DATOS DEL PIÑÓN, EN LA TABLA DE FACTORES J ES UN RECORRIDO HORIZONTAL i

            int NdientesPinonIgual = 0;  // cuando los numero de dientes  en el piñon son = Np, cuando si lo contiene la Db
            int NdientesPinonMenor = 0;  //                                  <Np
            int NdientesPinonMayor = 0;  //                                  >Np

            //* cada una de estas variables tiene acceso a 2 factores geométricos, factores J para corona y para piñón

            double JCoronaMax_i = 0;   // se extraen en la fila del piñón, su contenedor es NdientesPinonMayor
            double JPinonMax_i = 0;

            double JCoronaMin_i = 0;   // se extraen en la fila del piñón, su contenedor es NdientesPinonMenor
            double JPinonMin_i = 0;

            //double JCoronaIgual_i = 0;   // se extraen en la fila del piñón, su contenedor es NdientesPinonIgual
            //double JPinonIgual_i = 0;


            // VARIABES PARA EXTRAER LOS DATOS DE CORONA, EN LA TABLA DE FACTORES J ES UN RECORRIDO VERTICAL j


            int NdientesCoronaIgual = 0;  // cuando los numero de dientes  en la corona son = Ng
            int NdientesCoronaMayor = 0;     //                                               >Ng
            int NdientesCoronaMenor = 0;  //                                               <Ng

            //* cada una de estas variables tiene acceso a 2 factores geométricos, factores J para corona y para piñón

            double JCoronaMax_j = 0;   // se extraen en la columna corona, su contenedor es NdientesCoronaMayor
            double JPinonMax_j = 0;

            double JCoronaMin_j = 0;   // se extraen en la columna corona, su contenedor es NdientesCoronaMenor
            double JPinonMin_j = 0;

            //double JCoronaIgual_j = 0;   // se extraen en la columna corona, su contenedor es NdientesCoronaIgual
            //double JPinonIgual_j = 0;

            // SI EL NÚMERO DE DIENTES DEL PIÑON NO ESTÁ CONTENIDO EN LA DB

            int NdientesPinonMedio_i = Np;           // Se utilizan si El número de dientes del piñón no está
            double JPinonMedio_i = 0;       // contenido en en la Db
            double JCoronaMedio_i = 0;

            // SI EL NÚMERO DE DIENTES DE CORONA NO ESTÁ CONTENIDO EN LA DB

            int NdientesCoronaMedio_j = Ng;           // Se utilizan si El número de dientes de la corona no está
            double JPinonMedio_j = 0;       // contenido en la Db
            double JCoronaMedio_j = 0;

            // EL VALOR FINAL DE LOS FATORES J REQUERIDOS
            double J_PinonReal = 0;
            double J_CoronaReal = 0;

            // me da el valor del numero de dientes del piñón, igual al ingresado por la función, de ser el caso que haya
            string queri = @"SELECT Cantidad FROM Dientes WHERE Cantidad = @Np;";

            // me da el valor del numero de dientes de la corona, igual al ingresado por la función, de ser el caso que haya
            string queri2 = @"SELECT Cantidad FROM Dientes WHERE Cantidad = @Ng;";

            // me da el valor del Numero de dientes del piñón que es el menor mas cercano al numero de dientes del piñon ingresado
            string queri3 = @" SELECT Cantidad FROM Dientes WHERE Cantidad < @Np ORDER BY Cantidad DESC LIMIT 1;";

            // me da el valor del Numero de dientes del piñón que es el mayor mas cercano al numero de dientes del piñon ingresado
            string queri4 = @" SELECT Cantidad FROM Dientes WHERE Cantidad > @Np ORDER BY Cantidad ASC LIMIT 1;";

            // me da el valor del Numero de dientes de la corona que es el menor mas cercano al numero de dientes del piñon ingresado
            string queri5 = @" SELECT Cantidad FROM Dientes WHERE Cantidad < @Ng ORDER BY Cantidad DESC LIMIT 1;";

            // me da el valor del Numero de dientes de la corona que es el mayor mas cercano al numero de dientes del piñon ingresado
            string queri6 = @" SELECT Cantidad FROM Dientes WHERE Cantidad > @Ng ORDER BY Cantidad ASC LIMIT 1;";

            // me devuelve valores de los factores J para Piñón y Corona, con las claves Np, Ng, angulo de presión
            string queri7 = @"
                SELECT fj.J_Pinon, fj.J_Corona
                FROM FactorJ fj
                JOIN Dientes dp ON fj.IdPinon = dp.IdDiente
                JOIN Dientes dc ON fj.IdCorona = dc.IdDiente
                JOIN AnguloPresion ap ON fj.IdAngulo = ap.IdAngulo
                WHERE dp.Cantidad = @Np AND dc.Cantidad = @Ng AND ap.Grados = @Angulo;";


            using (var conexion = new SqliteConnection(_cadenaconexion))
            {
                conexion.Open();

                NdientesPinonIgual = SeleccionarNumeroDientes(queri, "@Np", conexion, Np);
                //...............................................................
                NdientesCoronaIgual = SeleccionarNumeroDientes(queri2, "@Ng", conexion, Ng);
                //...............................................................
                NdientesPinonMenor = SeleccionarNumeroDientes(queri3, "@Np", conexion, Np);
                //...............................................................
                NdientesPinonMayor = SeleccionarNumeroDientes(queri4, "@Np", conexion, Np);
                //...............................................................
                NdientesCoronaMenor = SeleccionarNumeroDientes(queri5, "@Ng", conexion, Ng);
                //...............................................................
                NdientesCoronaMayor = SeleccionarNumeroDientes(queri6, "@Ng", conexion, Ng);
                //...............................................................


                if (NdientesPinonIgual != 0 && NdientesCoronaIgual != 0) // si se encuentran los valores de Np, Ng calculados, en la DB
                {
                    (J_PinonReal, J_CoronaReal) = SeleccionarfactorJ(queri7, conexion, NdientesPinonIgual, NdientesCoronaIgual, anguloPresion);

                }

                // Cuando no encuentra Np en la fila de la tabla J, Busco uno con Numero de dientes mayor, con dientes menor,
                // como si hay el dato en la corona, entonces solo busco con el parámetro de la corona,
                // entonces será de interpolar los datos contenidos en (NdientesPinonMenor,NdientesCoronaIgual,anguloPresion)
                //                                                 con (NdientesPinonMayor,NdientesCoronaIgual,anguloPresion)  (Resultado final)
                else if (NdientesPinonIgual == 0 && NdientesCoronaIgual != 0)
                {
                    (JPinonMin_i, JCoronaMin_i) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMenor, NdientesCoronaIgual, anguloPresion);
                    (JPinonMax_i, JCoronaMax_i) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMayor, NdientesCoronaIgual, anguloPresion);


                    JPinonMedio_i = MathDeg.InterpolacionLineal(NdientesPinonMenor, JPinonMin_i, NdientesPinonMayor, JPinonMax_i, NdientesPinonMedio_i);
                    JCoronaMedio_i = MathDeg.InterpolacionLineal(NdientesPinonMenor, JCoronaMin_i, NdientesPinonMayor, JCoronaMax_i, NdientesPinonMedio_i);
                    J_PinonReal = JPinonMedio_i;
                    J_CoronaReal = JCoronaMedio_i;
                }


                // Cuando no encuentra Npg en la columna de la tabla J, Busco uno con Numero de dientes mayor, con dientes menor,
                // como si hay el dato en el piñón, entonces sslo busco con el parámetro del piñón,
                // entonces será de interpolar los datos contenidos en (NdientesPinonIgual,NdientesCoronaMenor,anguloPresion)
                //                                                 con (NdientesPinonIgual,NdientesCoronaMayor,anguloPresion)  (Resultado final)

                else if (NdientesPinonIgual != 0 && NdientesCoronaIgual == 0)
                {
                    (JPinonMin_j, JCoronaMin_j) = SeleccionarfactorJ(queri7, conexion, NdientesPinonIgual, NdientesCoronaMenor, anguloPresion);
                    (JPinonMax_j, JCoronaMax_j) = SeleccionarfactorJ(queri7, conexion, NdientesPinonIgual, NdientesCoronaMayor, anguloPresion);

                    JPinonMedio_j = MathDeg.InterpolacionLineal(NdientesCoronaMenor, JPinonMin_j, NdientesCoronaMayor, JPinonMax_j, NdientesCoronaMedio_j);
                    JCoronaMedio_j = MathDeg.InterpolacionLineal(NdientesCoronaMenor, JCoronaMin_j, NdientesCoronaMayor, JCoronaMax_j, NdientesCoronaMedio_j);

                    J_PinonReal = JPinonMedio_j;
                    J_CoronaReal = JCoronaMedio_j;

                }

                else if (NdientesPinonIgual == 0 && NdientesCoronaIgual == 0)
                {
                    // se guarda los valores en i con dientes de corona menor
                    (JPinonMin_i, JCoronaMin_i) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMenor, NdientesCoronaMenor, anguloPresion);
                    (JPinonMax_i, JCoronaMax_i) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMayor, NdientesCoronaMenor, anguloPresion);

                    JPinonMedio_i = MathDeg.InterpolacionLineal(NdientesPinonMenor, JPinonMin_i, NdientesPinonMayor, JPinonMax_i, NdientesPinonMedio_i);
                    JCoronaMedio_i = MathDeg.InterpolacionLineal(NdientesPinonMenor, JCoronaMin_i, NdientesPinonMayor, JCoronaMax_i, NdientesPinonMedio_i);

                    // solo en esta ocasión se utiliza estas variables j, para guardar una fila en i, con dientes de corona mayor.
                    (JPinonMin_j, JCoronaMin_j) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMenor, NdientesCoronaMayor, anguloPresion);
                    (JPinonMax_j, JCoronaMax_j) = SeleccionarfactorJ(queri7, conexion, NdientesPinonMayor, NdientesCoronaMayor, anguloPresion);

                    JPinonMedio_j = MathDeg.InterpolacionLineal(NdientesPinonMenor, JPinonMin_j, NdientesPinonMayor, JPinonMax_j, NdientesPinonMedio_i);
                    JCoronaMedio_j = MathDeg.InterpolacionLineal(NdientesPinonMenor, JCoronaMin_j, NdientesPinonMayor, JCoronaMax_j, NdientesPinonMedio_i);

                    // se interpolan los valores, que previamente han sido unterpolados en i
                    J_PinonReal = MathDeg.InterpolacionLineal(NdientesCoronaMenor, JPinonMedio_i, NdientesCoronaMayor, JPinonMedio_j, NdientesCoronaMedio_j);
                    J_CoronaReal = MathDeg.InterpolacionLineal(NdientesCoronaMenor, JCoronaMedio_i, NdientesCoronaMayor, JCoronaMedio_j, NdientesCoronaMedio_j);

                }

            }

            return (J_PinonReal, J_CoronaReal);
        }
        // Métodos internos solo para el método ObtenerFactoresJ
        private static int SeleccionarNumeroDientes(string query, string Valorcomando, SqliteConnection conexion, int NumeroDientes)
        {
            int valor = 0;
            SqliteCommand comando = new SqliteCommand(query, conexion);
            {
                comando.Parameters.AddWithValue(Valorcomando, NumeroDientes);
                {
                    using (var miReader = comando.ExecuteReader())
                    {
                        if (miReader.Read())
                        {
                            valor = miReader.IsDBNull(0) ? -1 : miReader.GetInt32(0);
                        }
                    }
                }
            }

            return valor;
        }

        // Método interno solo para el método obtenerFactoresJ, admite solo un tipo de instrctivo Db o query
        // Devuelve los factores J posicionados en: angulo de presion, Np, y Ng. Es genérico devovera si hay dato
        private static (double J_pinon, double J_corona) SeleccionarfactorJ(string query, SqliteConnection conexion, int Np, int Ng, int anguloPresion)
        {
            double J_Pinon = 0;
            double J_Corona = 0;
            using (var comando7 = new SqliteCommand(query, conexion))
            {
                comando7.Parameters.AddWithValue("@Np", Np);
                comando7.Parameters.AddWithValue("@Ng", Ng);
                comando7.Parameters.AddWithValue("@Angulo", anguloPresion);
                {
                    using (var miReader = comando7.ExecuteReader())
                    {
                        if (miReader.Read())
                        {
                            J_Pinon = miReader.IsDBNull(0) ? 0.0 : miReader.GetDouble(0);
                            J_Corona = miReader.IsDBNull(1) ? 0.0 : miReader.GetDouble(1);
                        }
                    }
                }
            }

            return (J_Pinon, J_Corona);

        }
        #endregion

        #region   Extracción de los valores de Factor Ka
        public List<TipoMaquina> ObtenerImpulsoras()
        {
            List<TipoMaquina> ValorImpulsoras = new List<TipoMaquina>();
            using var connection = new SqliteConnection(_cadenaconexion);
            connection.Open();
            using var comando = new SqliteCommand("SELECT id, nombre FROM TipoImpulsora", connection);
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                ValorImpulsoras.Add(new TipoMaquina
                {
                    ID = reader.GetInt32(0),
                    NOMBRE = reader.GetString(1)
                });
            }
            connection.Close();
            return ValorImpulsoras;
        }


        public List<TipoMaquina> ObtenerImpulsadas()
        {
            List<TipoMaquina> ValorImpulsadas = new List<TipoMaquina>();
            using var connection = new SqliteConnection(_cadenaconexion);
            connection.Open();
            using var comando = new SqliteCommand("SELECT id, nombre FROM TipoImpulsada", connection);
            using var reader = comando.ExecuteReader();
            while (reader.Read())
            {
                ValorImpulsadas.Add(new TipoMaquina
                {
                    ID = reader.GetInt32(0),
                    NOMBRE = reader.GetString(1)
                });
            }
            return ValorImpulsadas;
        }

        public double? ObtenerKaValor(int idImpulsora, int idImpulsada)
        {
            using var connection = new SqliteConnection(_cadenaconexion);
            connection.Open();
            var comando = new SqliteCommand("SELECT ka_valor FROM KaValores WHERE impulsora_id = @imp AND impulsada_id = @ida",connection);
            comando.Parameters.AddWithValue("@imp", idImpulsora);
            comando.Parameters.AddWithValue("@ida", idImpulsada);
            var result = comando.ExecuteScalar();
            return result != null ? Convert.ToDouble(result) : (double?)null;
        }

        #endregion

        #region  Obtención de los datos acerca de materiales

        public List<MaterialPropertyVM> GetMaterialProperties()
        {
            List<MaterialPropertyVM> DatosMaterial = new List<MaterialPropertyVM>();

            using (var connection = new SqliteConnection(_cadenaconexion)){
                connection.Open();
                string ComandoQuery = @"SELECT 
                                pm.id_propiedad,
                                m.nombre AS nombre_material,
                                pm.clase_agma,
                                pm.designacion,
                                t.descripcion AS tratamiento,
                                pm.resistencia_min,
                                pm.resistencia_max
                            FROM 
                                PropiedadesMateriales pm
                            JOIN 
                                Materiales m ON pm.id_material = m.id_material
                            JOIN 
                                Tratamientos t ON pm.id_tratamiento = t.id_tratamiento;";
                using (var comando = new SqliteCommand(ComandoQuery, connection))
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var propiedad = new MaterialPropertyVM()
                        {
                            IdPropiedad = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            NombreMaterial = reader.IsDBNull(1) ? "Sin nombre" : reader.GetString(1),
                            ClaseAgma = reader.IsDBNull(2) ? "N/A" : reader.GetString(2),
                            Designacion = reader.IsDBNull(3) ? "N/A" : reader.GetString(3),
                            Tratamiento = reader.IsDBNull(4) ? "Sin tratamiento" : reader.GetString(4),
                            ResistenciaMin_MPa = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                            ResistenciaMax_MPa = reader.IsDBNull(6) ? 0 : reader.GetDouble(6)
                        };

                        DatosMaterial.Add(propiedad);
                    }

                }
            }
            return DatosMaterial;
        
        }

        #endregion
    }

    // Clase que se utiliza solo para los métodos que obtienen en valor de Ka

    internal sealed class TipoMaquina
    {
        private string _nombre="";
        private int _id;
        public string NOMBRE{ get { return _nombre; } set { _nombre = value; } }
        public int ID { get { return _id; } set { _id = value; } }
    }
}
