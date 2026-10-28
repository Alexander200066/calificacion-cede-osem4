using System.Globalization;

namespace calificacion_cedeñosem4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StudentPicker.ItemsSource = new[]
            {
                "Ana López",
                "Carlos Pérez",
                "María González",
                "Juan Rodríguez",
                "Sofía Martínez"
            };
            StudentPicker.SelectedIndex = 0;
        }

        private async void OnCalculateClicked(object? sender, EventArgs e)
        {
            if (StudentPicker.SelectedItem is not string student)
            {
                await DisplayAlertAsync("Datos incompletos", "Seleccione un estudiante.", "Aceptar");
                return;
            }

            string? invalidField = null;
            var validGrades =
                TryReadGrade(Seguimiento1Entry.Text, out var seguimiento1) || (invalidField = "Nota Seguimiento 1") is null;
            validGrades &= TryReadGrade(Examen1Entry.Text, out var examen1) || (invalidField ??= "Examen 1") is null;
            validGrades &= TryReadGrade(Seguimiento2Entry.Text, out var seguimiento2) || (invalidField ??= "Nota Seguimiento 2") is null;
            validGrades &= TryReadGrade(Examen2Entry.Text, out var examen2) || (invalidField ??= "Examen 2") is null;

            if (!validGrades)
            {
                await DisplayAlertAsync("Dato inválido", $"{invalidField} debe ser un número entre 0 y 10.", "Aceptar");
                return;
            }

            var parcial1 = (seguimiento1 * 0.3) + (examen1 * 0.2);
            var parcial2 = (seguimiento2 * 0.3) + (examen2 * 0.2);
            var final = parcial1 + parcial2;
            var estado = final >= 7 ? "Aprobado" : final >= 5 ? "Complementario" : "REPROBADO";

            var resumen = $"Nombre: {student}\n" +
                          $"Fecha: {EvaluationDatePicker.Date:dd/MM/yyyy}\n" +
                          $"Nota Parcial 1: {parcial1:F2}\n" +
                          $"Nota Parcial 2: {parcial2:F2}\n" +
                          $"Nota Final: {final:F2}\n" +
                          $"Estado: {estado}";

            await DisplayAlertAsync("Resultado de calificaciones", resumen, "Aceptar");
        }

        private static bool TryReadGrade(string? text, out double grade)
        {
            var normalizedText = text?.Trim().Replace(',', '.');
            if (!double.TryParse(normalizedText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out grade) ||
                grade < 0 || grade > 10)
            {
                grade = 0;
                return false;
            }

            return true;
        }
    }
}
