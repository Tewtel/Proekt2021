﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Design.VisualBasicMigrationCodeGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace System.Data.Entity.Migrations.Design
{
  /// <summary>Generates VB.Net code for a code-based migration.</summary>
  public class VisualBasicMigrationCodeGenerator : MigrationCodeGenerator
  {
    private IEnumerable<Tuple<CreateTableOperation, AddForeignKeyOperation>> _newTableForeignKeys;
    private IEnumerable<Tuple<CreateTableOperation, CreateIndexOperation>> _newTableIndexes;

    /// <inheritdoc />
    public override ScaffoldedMigration Generate(
      string migrationId,
      IEnumerable<MigrationOperation> operations,
      string sourceModel,
      string targetModel,
      string @namespace,
      string className)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationId, nameof (migrationId));
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<MigrationOperation>>(operations, nameof (operations));
      System.Data.Entity.Utilities.Check.NotEmpty(targetModel, nameof (targetModel));
      System.Data.Entity.Utilities.Check.NotEmpty(className, nameof (className));
      className = this.ScrubName(className);
      this._newTableForeignKeys = (IEnumerable<Tuple<CreateTableOperation, AddForeignKeyOperation>>) operations.OfType<CreateTableOperation>().SelectMany((Func<CreateTableOperation, IEnumerable<AddForeignKeyOperation>>) (ct => operations.OfType<AddForeignKeyOperation>()), (ct, cfk) => new
      {
        ct = ct,
        cfk = cfk
      }).Where(_param1 => _param1.ct.Name.EqualsIgnoreCase(_param1.cfk.DependentTable)).Select(_param1 => Tuple.Create<CreateTableOperation, AddForeignKeyOperation>(_param1.ct, _param1.cfk)).ToList<Tuple<CreateTableOperation, AddForeignKeyOperation>>();
      this._newTableIndexes = (IEnumerable<Tuple<CreateTableOperation, CreateIndexOperation>>) operations.OfType<CreateTableOperation>().SelectMany((Func<CreateTableOperation, IEnumerable<CreateIndexOperation>>) (ct => operations.OfType<CreateIndexOperation>()), (ct, cfk) => new
      {
        ct = ct,
        cfk = cfk
      }).Where(_param1 => _param1.ct.Name.EqualsIgnoreCase(_param1.cfk.Table)).Select(_param1 => Tuple.Create<CreateTableOperation, CreateIndexOperation>(_param1.ct, _param1.cfk)).ToList<Tuple<CreateTableOperation, CreateIndexOperation>>();
      ScaffoldedMigration scaffoldedMigration = new ScaffoldedMigration()
      {
        MigrationId = migrationId,
        Language = "vb",
        UserCode = this.Generate(operations, @namespace, className),
        DesignerCode = this.Generate(migrationId, sourceModel, targetModel, @namespace, className)
      };
      if (!string.IsNullOrWhiteSpace(sourceModel))
        scaffoldedMigration.Resources.Add("Source", (object) sourceModel);
      scaffoldedMigration.Resources.Add("Target", (object) targetModel);
      return scaffoldedMigration;
    }

    /// <summary>
    /// Generates the primary code file that the user can view and edit.
    /// </summary>
    /// <param name="operations"> Operations to be performed by the migration. </param>
    /// <param name="namespace"> Namespace that code should be generated in. </param>
    /// <param name="className"> Name of the class that should be generated. </param>
    /// <returns> The generated code. </returns>
    protected virtual string Generate(
      IEnumerable<MigrationOperation> operations,
      string @namespace,
      string className)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<MigrationOperation>>(operations, nameof (operations));
      System.Data.Entity.Utilities.Check.NotEmpty(className, nameof (className));
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (IndentedTextWriter writer = new IndentedTextWriter((TextWriter) stringWriter))
        {
          this.WriteClassStart(@namespace, className, writer, "Inherits DbMigration", namespaces: this.GetNamespaces(operations));
          writer.WriteLine("Public Overrides Sub Up()");
          ++writer.Indent;
          ((IEnumerable<object>) operations.Except<MigrationOperation>((IEnumerable<MigrationOperation>) this._newTableForeignKeys.Select<Tuple<CreateTableOperation, AddForeignKeyOperation>, AddForeignKeyOperation>((Func<Tuple<CreateTableOperation, AddForeignKeyOperation>, AddForeignKeyOperation>) (t => t.Item2))).Except<MigrationOperation>((IEnumerable<MigrationOperation>) this._newTableIndexes.Select<Tuple<CreateTableOperation, CreateIndexOperation>, CreateIndexOperation>((Func<Tuple<CreateTableOperation, CreateIndexOperation>, CreateIndexOperation>) (t => t.Item2)))).Each<object>((Action<object>) (o =>
          {
            // ISSUE: reference to a compiler-generated field
            if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Action<CallSite, VisualBasicMigrationCodeGenerator, object, IndentedTextWriter>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, nameof (Generate), (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__0, this, o, writer);
          }));
          --writer.Indent;
          writer.WriteLine("End Sub");
          writer.WriteLine();
          writer.WriteLine("Public Overrides Sub Down()");
          ++writer.Indent;
          operations = operations.Select<MigrationOperation, MigrationOperation>((Func<MigrationOperation, MigrationOperation>) (o => o.Inverse)).Where<MigrationOperation>((Func<MigrationOperation, bool>) (o => o != null)).Reverse<MigrationOperation>();
          int num = operations.Any<MigrationOperation>((Func<MigrationOperation, bool>) (o => o is NotSupportedOperation)) ? 1 : 0;
          ((IEnumerable<object>) operations.Where<MigrationOperation>((Func<MigrationOperation, bool>) (o => !(o is NotSupportedOperation)))).Each<object>((Action<object>) (o =>
          {
            // ISSUE: reference to a compiler-generated field
            if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Action<CallSite, VisualBasicMigrationCodeGenerator, object, IndentedTextWriter>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, nameof (Generate), (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__1.Target((CallSite) VisualBasicMigrationCodeGenerator.\u003C\u003Eo__3.\u003C\u003Ep__1, this, o, writer);
          }));
          if (num != 0)
          {
            writer.Write("Throw New NotSupportedException(");
            writer.Write(this.Generate(System.Data.Entity.Resources.Strings.ScaffoldSprocInDownNotSupported));
            writer.WriteLine(")");
          }
          --writer.Indent;
          writer.WriteLine("End Sub");
          this.WriteClassEnd(@namespace, writer);
        }
        return stringWriter.ToString();
      }
    }

    /// <summary>
    /// Generates the code behind file with migration metadata.
    /// </summary>
    /// <param name="migrationId"> Unique identifier of the migration. </param>
    /// <param name="sourceModel"> Source model to be stored in the migration metadata. </param>
    /// <param name="targetModel"> Target model to be stored in the migration metadata. </param>
    /// <param name="namespace"> Namespace that code should be generated in. </param>
    /// <param name="className"> Name of the class that should be generated. </param>
    /// <returns> The generated code. </returns>
    protected virtual string Generate(
      string migrationId,
      string sourceModel,
      string targetModel,
      string @namespace,
      string className)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationId, nameof (migrationId));
      System.Data.Entity.Utilities.Check.NotEmpty(targetModel, nameof (targetModel));
      System.Data.Entity.Utilities.Check.NotEmpty(className, nameof (className));
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (IndentedTextWriter writer = new IndentedTextWriter((TextWriter) stringWriter))
        {
          writer.WriteLine("' <auto-generated />");
          this.WriteClassStart(@namespace, className, writer, "Implements IMigrationMetadata", true);
          writer.Write("Private ReadOnly Resources As New ResourceManager(GetType(");
          writer.Write(className);
          writer.WriteLine("))");
          writer.WriteLine();
          this.WriteProperty("Id", this.Quote(migrationId), writer);
          writer.WriteLine();
          this.WriteProperty("Source", sourceModel == null ? (string) null : "Resources.GetString(\"Source\")", writer);
          writer.WriteLine();
          this.WriteProperty("Target", "Resources.GetString(\"Target\")", writer);
          this.WriteClassEnd(@namespace, writer);
        }
        return stringWriter.ToString();
      }
    }

    /// <summary>
    /// Generates a property to return the source or target model in the code behind file.
    /// </summary>
    /// <param name="name"> Name of the property. </param>
    /// <param name="value"> Value to be returned. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void WriteProperty(string name, string value, IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("Private ReadOnly Property IMigrationMetadata_");
      writer.Write(name);
      writer.Write("() As String Implements IMigrationMetadata.");
      writer.WriteLine(name);
      ++writer.Indent;
      writer.WriteLine("Get");
      ++writer.Indent;
      writer.Write("Return ");
      writer.WriteLine(value ?? "Nothing");
      --writer.Indent;
      writer.WriteLine("End Get");
      --writer.Indent;
      writer.WriteLine("End Property");
    }

    /// <summary>Generates class attributes.</summary>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    /// <param name="designer"> A value indicating if this class is being generated for a code-behind file. </param>
    protected virtual void WriteClassAttributes(IndentedTextWriter writer, bool designer)
    {
      if (!designer)
        return;
      writer.WriteLine("<GeneratedCode(\"EntityFramework.Migrations\", \"{0}\")>", (object) typeof (VisualBasicMigrationCodeGenerator).Assembly().GetInformationalVersion());
    }

    /// <summary>
    /// Generates a namespace, using statements and class definition.
    /// </summary>
    /// <param name="namespace"> Namespace that code should be generated in. </param>
    /// <param name="className"> Name of the class that should be generated. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    /// <param name="base"> Base class for the generated class. </param>
    /// <param name="designer"> A value indicating if this class is being generated for a code-behind file. </param>
    /// <param name="namespaces"> Namespaces for which Imports directives will be added. If null, then the namespaces returned from GetDefaultNamespaces will be used. </param>
    protected virtual void WriteClassStart(
      string @namespace,
      string className,
      IndentedTextWriter writer,
      string @base,
      bool designer = false,
      IEnumerable<string> namespaces = null)
    {
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      System.Data.Entity.Utilities.Check.NotEmpty(className, nameof (className));
      System.Data.Entity.Utilities.Check.NotEmpty(@base, nameof (@base));
      (namespaces ?? this.GetDefaultNamespaces(designer)).Each<string>((Action<string>) (n => writer.WriteLine("Imports " + n)));
      if (!designer)
        writer.WriteLine("Imports Microsoft.VisualBasic");
      writer.WriteLine();
      if (!string.IsNullOrWhiteSpace(@namespace))
      {
        writer.Write("Namespace ");
        writer.WriteLine(@namespace);
        ++writer.Indent;
      }
      this.WriteClassAttributes(writer, designer);
      writer.Write("Public ");
      if (designer)
        writer.Write("NotInheritable ");
      writer.Write("Partial Class ");
      writer.Write(className);
      writer.WriteLine();
      ++writer.Indent;
      writer.WriteLine(@base);
      --writer.Indent;
      writer.WriteLine();
      ++writer.Indent;
    }

    /// <summary>
    /// Generates the closing code for a class that was started with WriteClassStart.
    /// </summary>
    /// <param name="namespace"> Namespace that code should be generated in. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void WriteClassEnd(string @namespace, IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      --writer.Indent;
      writer.WriteLine("End Class");
      if (string.IsNullOrWhiteSpace(@namespace))
        return;
      --writer.Indent;
      writer.WriteLine("End Namespace");
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddColumnOperation" />.
    /// </summary>
    /// <param name="addColumnOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      AddColumnOperation addColumnOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AddColumnOperation>(addColumnOperation, nameof (addColumnOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("AddColumn(");
      writer.Write(this.Quote(addColumnOperation.Table));
      writer.Write(", ");
      writer.Write(this.Quote(addColumnOperation.Column.Name));
      writer.Write(", Function(c)");
      this.Generate(addColumnOperation.Column, writer);
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropColumnOperation" />.
    /// </summary>
    /// <param name="dropColumnOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      DropColumnOperation dropColumnOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropColumnOperation>(dropColumnOperation, nameof (dropColumnOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropColumn(");
      writer.Write(this.Quote(dropColumnOperation.Table));
      writer.Write(", ");
      writer.Write(this.Quote(dropColumnOperation.Name));
      if (dropColumnOperation.RemovedAnnotations.Any<KeyValuePair<string, object>>())
      {
        ++writer.Indent;
        writer.WriteLine(",");
        writer.Write("removedAnnotations := ");
        this.GenerateAnnotations(dropColumnOperation.RemovedAnnotations, writer);
        --writer.Indent;
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AlterColumnOperation" />.
    /// </summary>
    /// <param name="alterColumnOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      AlterColumnOperation alterColumnOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AlterColumnOperation>(alterColumnOperation, nameof (alterColumnOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("AlterColumn(");
      writer.Write(this.Quote(alterColumnOperation.Table));
      writer.Write(", ");
      writer.Write(this.Quote(alterColumnOperation.Column.Name));
      writer.Write(", Function(c)");
      this.Generate(alterColumnOperation.Column, writer);
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code for to re-create the given dictionary of annotations for use when passing
    /// these annotations as a parameter of a <see cref="T:System.Data.Entity.Migrations.DbMigration" />. call.
    /// </summary>
    /// <param name="annotations">The annotations to generate.</param>
    /// <param name="writer">The writer to which generated code should be written.</param>
    protected internal virtual void GenerateAnnotations(
      IDictionary<string, object> annotations,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDictionary<string, object>>(annotations, nameof (annotations));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine("New Dictionary(Of String, Object)() From _");
      writer.WriteLine("{");
      ++writer.Indent;
      string[] array = annotations.Keys.OrderBy<string, string>((Func<string, string>) (k => k)).ToArray<string>();
      for (int index = 0; index < array.Length; ++index)
      {
        writer.Write("{ ");
        writer.Write(this.Quote(array[index]) + ", ");
        this.GenerateAnnotation(array[index], annotations[array[index]], writer);
        writer.WriteLine(index < array.Length - 1 ? " }," : " }");
      }
      --writer.Indent;
      writer.Write("}");
    }

    /// <summary>
    /// Generates code for to re-create the given dictionary of annotations for use when passing
    /// these annotations as a parameter of a <see cref="T:System.Data.Entity.Migrations.DbMigration" />. call.
    /// </summary>
    /// <param name="annotations">The annotations to generate.</param>
    /// <param name="writer">The writer to which generated code should be written.</param>
    protected internal virtual void GenerateAnnotations(
      IDictionary<string, AnnotationValues> annotations,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDictionary<string, AnnotationValues>>(annotations, nameof (annotations));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine("New Dictionary(Of String, AnnotationValues)() From _");
      writer.WriteLine("{");
      ++writer.Indent;
      if (annotations != null)
      {
        string[] array = annotations.Keys.OrderBy<string, string>((Func<string, string>) (k => k)).ToArray<string>();
        for (int index = 0; index < array.Length; ++index)
        {
          writer.WriteLine("{");
          ++writer.Indent;
          writer.WriteLine(this.Quote(array[index]) + ",");
          writer.Write("New AnnotationValues(oldValue := ");
          this.GenerateAnnotation(array[index], annotations[array[index]].OldValue, writer);
          writer.Write(", newValue := ");
          this.GenerateAnnotation(array[index], annotations[array[index]].NewValue, writer);
          writer.WriteLine(")");
          --writer.Indent;
          writer.WriteLine(index < array.Length - 1 ? " }," : " }");
        }
      }
      --writer.Indent;
      writer.Write("}");
    }

    /// <summary>
    /// Generates code for the given annotation value, which may be null. The default behavior is to use an
    /// <see cref="T:System.Data.Entity.Infrastructure.Annotations.AnnotationCodeGenerator" /> if one is registered, otherwise call ToString on the annotation value.
    /// </summary>
    /// <remarks>
    /// Note that a <see cref="T:System.Data.Entity.Infrastructure.Annotations.AnnotationCodeGenerator" /> can be registered to generate code for custom annotations
    /// without the need to override the entire code generator.
    /// </remarks>
    /// <param name="name">The name of the annotation for which code is needed.</param>
    /// <param name="annotation">The annotation value to generate.</param>
    /// <param name="writer">The writer to which generated code should be written.</param>
    protected internal virtual void GenerateAnnotation(
      string name,
      object annotation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      if (annotation == null)
      {
        writer.Write("Nothing");
      }
      else
      {
        Func<AnnotationCodeGenerator> func;
        if (this.AnnotationGenerators.TryGetValue(name, out func) && func != null)
          func().Generate(name, annotation, writer);
        else
          writer.Write(this.Quote(annotation.ToString()));
      }
    }

    /// <summary>Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateProcedureOperation" />.</summary>
    /// <param name="createProcedureOperation">The operation to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    protected virtual void Generate(
      CreateProcedureOperation createProcedureOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<CreateProcedureOperation>(createProcedureOperation, nameof (createProcedureOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      this.Generate((ProcedureOperation) createProcedureOperation, "CreateStoredProcedure", writer);
    }

    /// <summary>Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.AlterProcedureOperation" />.</summary>
    /// <param name="alterProcedureOperation">The operation to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    protected virtual void Generate(
      AlterProcedureOperation alterProcedureOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AlterProcedureOperation>(alterProcedureOperation, nameof (alterProcedureOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      this.Generate((ProcedureOperation) alterProcedureOperation, "AlterStoredProcedure", writer);
    }

    private void Generate(
      ProcedureOperation procedureOperation,
      string methodName,
      IndentedTextWriter writer)
    {
      writer.Write(methodName);
      writer.WriteLine("(");
      ++writer.Indent;
      writer.Write(this.Quote(procedureOperation.Name));
      writer.WriteLine(",");
      if (procedureOperation.Parameters.Any<ParameterModel>())
      {
        writer.WriteLine("Function(p) New With");
        ++writer.Indent;
        writer.WriteLine("{");
        ++writer.Indent;
        procedureOperation.Parameters.Each<ParameterModel>((Action<ParameterModel, int>) ((p, i) =>
        {
          string b = this.ScrubName(p.Name);
          writer.Write(".");
          writer.Write(b);
          writer.Write(" =");
          this.Generate(p, writer, !string.Equals(p.Name, b, StringComparison.Ordinal));
          if (i < procedureOperation.Parameters.Count - 1)
            writer.Write(",");
          writer.WriteLine();
        }));
        --writer.Indent;
        writer.WriteLine("},");
        --writer.Indent;
      }
      writer.Write("body :=");
      if (!string.IsNullOrWhiteSpace(procedureOperation.BodySql))
      {
        writer.WriteLine();
        ++writer.Indent;
        string newValue = "\" & vbCrLf & _" + writer.NewLine + writer.CurrentIndentation() + "\"";
        writer.WriteLine(this.Generate(procedureOperation.BodySql.Replace(Environment.NewLine, newValue)));
        --writer.Indent;
      }
      else
        writer.WriteLine(" \"\"");
      --writer.Indent;
      writer.WriteLine(")");
      writer.WriteLine();
    }

    /// <summary>Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.ParameterModel" />.</summary>
    /// <param name="parameterModel">The parameter model definition to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    /// <param name="emitName">true to include the column name in the definition; otherwise, false.</param>
    protected virtual void Generate(
      ParameterModel parameterModel,
      IndentedTextWriter writer,
      bool emitName = false)
    {
      System.Data.Entity.Utilities.Check.NotNull<ParameterModel>(parameterModel, nameof (parameterModel));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write(" p.");
      writer.Write(this.TranslateColumnType(parameterModel.Type));
      writer.Write("(");
      List<string> ts = new List<string>();
      if (emitName)
        ts.Add("name := " + this.Quote(parameterModel.Name));
      int? maxLength = parameterModel.MaxLength;
      if (maxLength.HasValue)
      {
        List<string> stringList = ts;
        maxLength = parameterModel.MaxLength;
        string str = "maxLength := " + maxLength.ToString();
        stringList.Add(str);
      }
      byte? nullable1 = parameterModel.Precision;
      if (nullable1.HasValue)
      {
        List<string> stringList = ts;
        nullable1 = parameterModel.Precision;
        string str = "precision := " + nullable1.ToString();
        stringList.Add(str);
      }
      nullable1 = parameterModel.Scale;
      if (nullable1.HasValue)
      {
        List<string> stringList = ts;
        nullable1 = parameterModel.Scale;
        string str = "scale := " + nullable1.ToString();
        stringList.Add(str);
      }
      bool? nullable2 = parameterModel.IsFixedLength;
      if (nullable2.HasValue)
      {
        List<string> stringList = ts;
        nullable2 = parameterModel.IsFixedLength;
        string str = "fixedLength := " + nullable2.ToString().ToLowerInvariant();
        stringList.Add(str);
      }
      nullable2 = parameterModel.IsUnicode;
      if (nullable2.HasValue)
      {
        List<string> stringList = ts;
        nullable2 = parameterModel.IsUnicode;
        string str = "unicode := " + nullable2.ToString().ToLowerInvariant();
        stringList.Add(str);
      }
      if (parameterModel.DefaultValue != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__2 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, List<string>, object> target1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, List<string>, object>> p2 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__2;
        List<string> stringList = ts;
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, string, object, object> target2 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, string, object, object>> p1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Func<CallSite, VisualBasicMigrationCodeGenerator, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, nameof (Generate), (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) VisualBasicMigrationCodeGenerator.\u003C\u003Eo__18.\u003C\u003Ep__0, this, parameterModel.DefaultValue);
        object obj2 = target2((CallSite) p1, "defaultValue := ", obj1);
        target1((CallSite) p2, stringList, obj2);
      }
      if (!string.IsNullOrWhiteSpace(parameterModel.DefaultValueSql))
        ts.Add("defaultValueSql := " + this.Quote(parameterModel.DefaultValueSql));
      if (!string.IsNullOrWhiteSpace(parameterModel.StoreType))
        ts.Add("storeType := " + this.Quote(parameterModel.StoreType));
      if (parameterModel.IsOutParameter)
        ts.Add("outParameter := True");
      writer.Write(ts.Join<string>());
      writer.Write(")");
    }

    /// <summary>Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropProcedureOperation" />.</summary>
    /// <param name="dropProcedureOperation">The operation to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    protected virtual void Generate(
      DropProcedureOperation dropProcedureOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropProcedureOperation>(dropProcedureOperation, nameof (dropProcedureOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropStoredProcedure(");
      writer.Write(this.Quote(dropProcedureOperation.Name));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateTableOperation" />.
    /// </summary>
    /// <param name="createTableOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      CreateTableOperation createTableOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<CreateTableOperation>(createTableOperation, nameof (createTableOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine("CreateTable(");
      ++writer.Indent;
      writer.Write(this.Quote(createTableOperation.Name));
      writer.WriteLine(",");
      writer.WriteLine("Function(c) New With");
      ++writer.Indent;
      writer.WriteLine("{");
      ++writer.Indent;
      int columnCount = createTableOperation.Columns.Count<ColumnModel>();
      createTableOperation.Columns.Each<ColumnModel>((Action<ColumnModel, int>) ((c, i) =>
      {
        string b = this.ScrubName(c.Name);
        writer.Write(".");
        writer.Write(b);
        writer.Write(" =");
        this.Generate(c, writer, !string.Equals(c.Name, b, StringComparison.Ordinal));
        if (i < columnCount - 1)
          writer.Write(",");
        writer.WriteLine();
      }));
      --writer.Indent;
      writer.Write("}");
      --writer.Indent;
      if (createTableOperation.Annotations.Any<KeyValuePair<string, object>>())
      {
        writer.WriteLine(",");
        writer.Write("annotations := ");
        this.GenerateAnnotations(createTableOperation.Annotations, writer);
      }
      writer.Write(")");
      this.GenerateInline(createTableOperation.PrimaryKey, writer);
      this._newTableForeignKeys.Where<Tuple<CreateTableOperation, AddForeignKeyOperation>>((Func<Tuple<CreateTableOperation, AddForeignKeyOperation>, bool>) (t => t.Item1 == createTableOperation)).Each<Tuple<CreateTableOperation, AddForeignKeyOperation>>((Action<Tuple<CreateTableOperation, AddForeignKeyOperation>>) (t => this.GenerateInline(t.Item2, writer)));
      this._newTableIndexes.Where<Tuple<CreateTableOperation, CreateIndexOperation>>((Func<Tuple<CreateTableOperation, CreateIndexOperation>, bool>) (t => t.Item1 == createTableOperation)).Each<Tuple<CreateTableOperation, CreateIndexOperation>>((Action<Tuple<CreateTableOperation, CreateIndexOperation>>) (t => this.GenerateInline(t.Item2, writer)));
      writer.WriteLine();
      --writer.Indent;
      writer.WriteLine();
    }

    /// <summary>
    /// Generates code for an <see cref="T:System.Data.Entity.Migrations.Model.AlterTableOperation" />.
    /// </summary>
    /// <param name="alterTableOperation">The operation for which code should be generated.</param>
    /// <param name="writer">The writer to which generated code should be written.</param>
    protected internal virtual void Generate(
      AlterTableOperation alterTableOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AlterTableOperation>(alterTableOperation, nameof (alterTableOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine("AlterTableAnnotations(");
      ++writer.Indent;
      writer.Write(this.Quote(alterTableOperation.Name));
      writer.WriteLine(",");
      writer.WriteLine("Function(c) New With");
      ++writer.Indent;
      writer.WriteLine("{");
      ++writer.Indent;
      int columnCount = alterTableOperation.Columns.Count<ColumnModel>();
      alterTableOperation.Columns.Each<ColumnModel>((Action<ColumnModel, int>) ((c, i) =>
      {
        string b = this.ScrubName(c.Name);
        writer.Write(".");
        writer.Write(b);
        writer.Write(" =");
        this.Generate(c, writer, !string.Equals(c.Name, b, StringComparison.Ordinal));
        if (i < columnCount - 1)
          writer.Write(",");
        writer.WriteLine();
      }));
      --writer.Indent;
      writer.Write("}");
      --writer.Indent;
      if (alterTableOperation.Annotations.Any<KeyValuePair<string, AnnotationValues>>())
      {
        writer.WriteLine(",");
        writer.Write("annotations := ");
        this.GenerateAnnotations(alterTableOperation.Annotations, writer);
      }
      writer.Write(")");
      writer.WriteLine();
      --writer.Indent;
      writer.WriteLine();
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddPrimaryKeyOperation" /> as part of a <see cref="T:System.Data.Entity.Migrations.Model.CreateTableOperation" />.
    /// </summary>
    /// <param name="addPrimaryKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void GenerateInline(
      AddPrimaryKeyOperation addPrimaryKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      if (addPrimaryKeyOperation == null)
        return;
      writer.WriteLine(" _");
      writer.Write(".PrimaryKey(");
      this.Generate((IEnumerable<string>) addPrimaryKeyOperation.Columns, writer);
      if (!addPrimaryKeyOperation.HasDefaultName)
      {
        writer.Write(", name := ");
        writer.Write(this.Quote(addPrimaryKeyOperation.Name));
      }
      if (!addPrimaryKeyOperation.IsClustered)
        writer.Write(", clustered := False");
      writer.Write(")");
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddForeignKeyOperation" /> as part of a <see cref="T:System.Data.Entity.Migrations.Model.CreateTableOperation" />.
    /// </summary>
    /// <param name="addForeignKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void GenerateInline(
      AddForeignKeyOperation addForeignKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AddForeignKeyOperation>(addForeignKeyOperation, nameof (addForeignKeyOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine(" _");
      writer.Write(".ForeignKey(" + this.Quote(addForeignKeyOperation.PrincipalTable) + ", ");
      this.Generate((IEnumerable<string>) addForeignKeyOperation.DependentColumns, writer);
      if (addForeignKeyOperation.CascadeDelete)
        writer.Write(", cascadeDelete := True");
      writer.Write(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateIndexOperation" /> as part of a <see cref="T:System.Data.Entity.Migrations.Model.CreateTableOperation" />.
    /// </summary>
    /// <param name="createIndexOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void GenerateInline(
      CreateIndexOperation createIndexOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<CreateIndexOperation>(createIndexOperation, nameof (createIndexOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.WriteLine(" _");
      writer.Write(".Index(");
      this.Generate((IEnumerable<string>) createIndexOperation.Columns, writer);
      this.WriteIndexParameters(createIndexOperation, writer);
      writer.Write(")");
    }

    /// <summary>
    /// Generates code to specify a set of column names using a lambda expression.
    /// </summary>
    /// <param name="columns"> The columns to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(IEnumerable<string> columns, IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<string>>(columns, nameof (columns));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("Function(t) ");
      if (columns.Count<string>() == 1)
        writer.Write("t." + this.ScrubName(columns.Single<string>()));
      else
        writer.Write("New With { " + columns.Join<string>((Func<string, string>) (c => "t." + this.ScrubName(c))) + " }");
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddForeignKeyOperation" />.
    /// </summary>
    /// <param name="addForeignKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      AddForeignKeyOperation addForeignKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AddForeignKeyOperation>(addForeignKeyOperation, nameof (addForeignKeyOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("AddForeignKey(");
      writer.Write(this.Quote(addForeignKeyOperation.DependentTable));
      writer.Write(", ");
      bool flag = addForeignKeyOperation.DependentColumns.Count<string>() > 1;
      if (flag)
        writer.Write("New String() { ");
      writer.Write(addForeignKeyOperation.DependentColumns.Join<string>(new Func<string, string>(this.Quote)));
      if (flag)
        writer.Write(" }");
      writer.Write(", ");
      writer.Write(this.Quote(addForeignKeyOperation.PrincipalTable));
      if (addForeignKeyOperation.PrincipalColumns.Any<string>())
      {
        writer.Write(", ");
        if (flag)
          writer.Write("New String() { ");
        writer.Write(addForeignKeyOperation.PrincipalColumns.Join<string>(new Func<string, string>(this.Quote)));
        if (flag)
          writer.Write(" }");
      }
      if (addForeignKeyOperation.CascadeDelete)
        writer.Write(", cascadeDelete := True");
      if (!addForeignKeyOperation.HasDefaultName)
      {
        writer.Write(", name := ");
        writer.Write(this.Quote(addForeignKeyOperation.Name));
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropForeignKeyOperation" />.
    /// </summary>
    /// <param name="dropForeignKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      DropForeignKeyOperation dropForeignKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropForeignKeyOperation>(dropForeignKeyOperation, nameof (dropForeignKeyOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropForeignKey(");
      writer.Write(this.Quote(dropForeignKeyOperation.DependentTable));
      writer.Write(", ");
      if (!dropForeignKeyOperation.HasDefaultName)
      {
        writer.Write(this.Quote(dropForeignKeyOperation.Name));
      }
      else
      {
        int num = dropForeignKeyOperation.DependentColumns.Count<string>() > 1 ? 1 : 0;
        if (num != 0)
          writer.Write("New String() { ");
        writer.Write(dropForeignKeyOperation.DependentColumns.Join<string>(new Func<string, string>(this.Quote)));
        if (num != 0)
          writer.Write(" }");
        writer.Write(", ");
        writer.Write(this.Quote(dropForeignKeyOperation.PrincipalTable));
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform an <see cref="T:System.Data.Entity.Migrations.Model.AddPrimaryKeyOperation" />.
    /// </summary>
    /// <param name="addPrimaryKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      AddPrimaryKeyOperation addPrimaryKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<AddPrimaryKeyOperation>(addPrimaryKeyOperation, nameof (addPrimaryKeyOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("AddPrimaryKey(");
      writer.Write(this.Quote(addPrimaryKeyOperation.Table));
      writer.Write(", ");
      int num = addPrimaryKeyOperation.Columns.Count<string>() > 1 ? 1 : 0;
      if (num != 0)
        writer.Write("New String() { ");
      writer.Write(addPrimaryKeyOperation.Columns.Join<string>(new Func<string, string>(this.Quote)));
      if (num != 0)
        writer.Write(" }");
      if (!addPrimaryKeyOperation.HasDefaultName)
      {
        writer.Write(", name := ");
        writer.Write(this.Quote(addPrimaryKeyOperation.Name));
      }
      if (!addPrimaryKeyOperation.IsClustered)
        writer.Write(", clustered := False");
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropPrimaryKeyOperation" />.
    /// </summary>
    /// <param name="dropPrimaryKeyOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      DropPrimaryKeyOperation dropPrimaryKeyOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropPrimaryKeyOperation>(dropPrimaryKeyOperation, nameof (dropPrimaryKeyOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropPrimaryKey(");
      writer.Write(this.Quote(dropPrimaryKeyOperation.Table));
      if (!dropPrimaryKeyOperation.HasDefaultName)
      {
        writer.Write(", name := ");
        writer.Write(this.Quote(dropPrimaryKeyOperation.Name));
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.CreateIndexOperation" />.
    /// </summary>
    /// <param name="createIndexOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      CreateIndexOperation createIndexOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<CreateIndexOperation>(createIndexOperation, nameof (createIndexOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("CreateIndex(");
      writer.Write(this.Quote(createIndexOperation.Table));
      writer.Write(", ");
      int num = createIndexOperation.Columns.Count<string>() > 1 ? 1 : 0;
      if (num != 0)
        writer.Write("New String() { ");
      writer.Write(createIndexOperation.Columns.Join<string>(new Func<string, string>(this.Quote)));
      if (num != 0)
        writer.Write(" }");
      this.WriteIndexParameters(createIndexOperation, writer);
      writer.WriteLine(")");
    }

    private void WriteIndexParameters(
      CreateIndexOperation createIndexOperation,
      IndentedTextWriter writer)
    {
      if (createIndexOperation.IsUnique)
        writer.Write(", unique := True");
      if (createIndexOperation.IsClustered)
        writer.Write(", clustered := True");
      if (createIndexOperation.HasDefaultName)
        return;
      writer.Write(", name := ");
      writer.Write(this.Quote(createIndexOperation.Name));
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropIndexOperation" />.
    /// </summary>
    /// <param name="dropIndexOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      DropIndexOperation dropIndexOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropIndexOperation>(dropIndexOperation, nameof (dropIndexOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropIndex(");
      writer.Write(this.Quote(dropIndexOperation.Table));
      writer.Write(", ");
      if (!dropIndexOperation.HasDefaultName)
      {
        writer.Write(this.Quote(dropIndexOperation.Name));
      }
      else
      {
        writer.Write("New String() { ");
        writer.Write(dropIndexOperation.Columns.Join<string>(new Func<string, string>(this.Quote)));
        writer.Write(" }");
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to specify the definition for a <see cref="T:System.Data.Entity.Migrations.Model.ColumnModel" />.
    /// </summary>
    /// <param name="column"> The column definition to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    /// <param name="emitName"> A value indicating whether to include the column name in the definition. </param>
    protected virtual void Generate(ColumnModel column, IndentedTextWriter writer, bool emitName = false)
    {
      System.Data.Entity.Utilities.Check.NotNull<ColumnModel>(column, nameof (column));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write(" c.");
      writer.Write(this.TranslateColumnType(column.Type));
      writer.Write("(");
      List<string> stringList1 = new List<string>();
      if (emitName)
        stringList1.Add("name := " + this.Quote(column.Name));
      bool? isNullable = column.IsNullable;
      bool flag = false;
      if (isNullable.GetValueOrDefault() == flag & isNullable.HasValue)
        stringList1.Add("nullable := False");
      int? maxLength = column.MaxLength;
      if (maxLength.HasValue)
      {
        List<string> stringList2 = stringList1;
        maxLength = column.MaxLength;
        string str = "maxLength := " + maxLength.ToString();
        stringList2.Add(str);
      }
      byte? nullable1 = column.Precision;
      if (nullable1.HasValue)
      {
        List<string> stringList2 = stringList1;
        nullable1 = column.Precision;
        string str = "precision := " + nullable1.ToString();
        stringList2.Add(str);
      }
      nullable1 = column.Scale;
      if (nullable1.HasValue)
      {
        List<string> stringList2 = stringList1;
        nullable1 = column.Scale;
        string str = "scale := " + nullable1.ToString();
        stringList2.Add(str);
      }
      bool? nullable2 = column.IsFixedLength;
      if (nullable2.HasValue)
      {
        List<string> stringList2 = stringList1;
        nullable2 = column.IsFixedLength;
        string str = "fixedLength := " + nullable2.ToString().ToLowerInvariant();
        stringList2.Add(str);
      }
      nullable2 = column.IsUnicode;
      if (nullable2.HasValue)
      {
        List<string> stringList2 = stringList1;
        nullable2 = column.IsUnicode;
        string str = "unicode := " + nullable2.ToString().ToLowerInvariant();
        stringList2.Add(str);
      }
      if (column.IsIdentity)
        stringList1.Add("identity := True");
      if (column.DefaultValue != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__2 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, List<string>, object> target1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, List<string>, object>> p2 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__2;
        List<string> stringList2 = stringList1;
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__1 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, string, object, object> target2 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, string, object, object>> p1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__0 = CallSite<Func<CallSite, VisualBasicMigrationCodeGenerator, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, nameof (Generate), (IEnumerable<Type>) null, typeof (VisualBasicMigrationCodeGenerator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__0.Target((CallSite) VisualBasicMigrationCodeGenerator.\u003C\u003Eo__33.\u003C\u003Ep__0, this, column.DefaultValue);
        object obj2 = target2((CallSite) p1, "defaultValue := ", obj1);
        target1((CallSite) p2, stringList2, obj2);
      }
      if (!string.IsNullOrWhiteSpace(column.DefaultValueSql))
        stringList1.Add("defaultValueSql := " + this.Quote(column.DefaultValueSql));
      if (column.IsTimestamp)
        stringList1.Add("timestamp := True");
      if (!string.IsNullOrWhiteSpace(column.StoreType))
        stringList1.Add("storeType := " + this.Quote(column.StoreType));
      writer.Write(stringList1.Join<string>());
      if (column.Annotations.Any<KeyValuePair<string, AnnotationValues>>())
      {
        ++writer.Indent;
        writer.WriteLine(stringList1.Any<string>() ? "," : "");
        writer.Write("annotations := ");
        this.GenerateAnnotations(column.Annotations, writer);
        --writer.Indent;
      }
      writer.Write(")");
    }

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Byte[]" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(byte[] defaultValue) => "New Byte() {" + ((IEnumerable<byte>) defaultValue).Join<byte>() + "}";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.DateTime" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(DateTime defaultValue) => "New DateTime(" + defaultValue.Ticks.ToString() + ", DateTimeKind." + Enum.GetName(typeof (DateTimeKind), (object) defaultValue.Kind) + ")";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.DateTimeOffset" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(DateTimeOffset defaultValue) => "New DateTimeOffset(" + defaultValue.Ticks.ToString() + ", new TimeSpan(" + defaultValue.Offset.Ticks.ToString() + "))";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Decimal" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(Decimal defaultValue) => defaultValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "D";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Guid" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(Guid defaultValue) => "New Guid(\"" + defaultValue.ToString() + "\")";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Int64" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(long defaultValue) => defaultValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Single" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(float defaultValue) => defaultValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "F";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.String" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(string defaultValue) => this.Quote(defaultValue);

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.TimeSpan" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(TimeSpan defaultValue) => "New TimeSpan(" + defaultValue.Ticks.ToString() + ")";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Data.Entity.Hierarchy.HierarchyId" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(HierarchyId defaultValue) => "New HierarchyId(\"" + defaultValue?.ToString() + "\")";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Data.Entity.Spatial.DbGeography" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(DbGeography defaultValue) => "DbGeography.FromText(\"" + defaultValue.AsText() + "\", " + defaultValue.CoordinateSystemId.ToString() + ")";

    /// <summary>
    /// Generates code to specify the default value for a <see cref="T:System.Data.Entity.Spatial.DbGeometry" /> column.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(DbGeometry defaultValue) => "DbGeometry.FromText(\"" + defaultValue.AsText() + "\", " + defaultValue.CoordinateSystemId.ToString() + ")";

    /// <summary>
    /// Generates code to specify the default value for a column of unknown data type.
    /// </summary>
    /// <param name="defaultValue"> The value to be used as the default. </param>
    /// <returns> Code representing the default value. </returns>
    protected virtual string Generate(object defaultValue) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}", defaultValue).ToLowerInvariant();

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.DropTableOperation" />.
    /// </summary>
    /// <param name="dropTableOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      DropTableOperation dropTableOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DropTableOperation>(dropTableOperation, nameof (dropTableOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("DropTable(");
      writer.Write(this.Quote(dropTableOperation.Name));
      if (dropTableOperation.RemovedAnnotations.Any<KeyValuePair<string, object>>())
      {
        ++writer.Indent;
        writer.WriteLine(",");
        writer.Write("removedAnnotations := ");
        this.GenerateAnnotations(dropTableOperation.RemovedAnnotations, writer);
        --writer.Indent;
      }
      IDictionary<string, IDictionary<string, object>> columnAnnotations = dropTableOperation.RemovedColumnAnnotations;
      if (columnAnnotations.Any<KeyValuePair<string, IDictionary<string, object>>>())
      {
        ++writer.Indent;
        writer.WriteLine(",");
        writer.Write("removedColumnAnnotations := ");
        writer.WriteLine("New Dictionary(Of String, IDictionary(Of String, Object)) From _");
        writer.WriteLine("{");
        ++writer.Indent;
        string[] array = columnAnnotations.Keys.OrderBy<string, string>((Func<string, string>) (k => k)).ToArray<string>();
        for (int index = 0; index < array.Length; ++index)
        {
          writer.WriteLine("{");
          ++writer.Indent;
          writer.WriteLine(this.Quote(array[index]) + ",");
          this.GenerateAnnotations(columnAnnotations[array[index]], writer);
          writer.WriteLine();
          --writer.Indent;
          writer.WriteLine(index < array.Length - 1 ? " }," : " }");
        }
        --writer.Indent;
        writer.Write("}");
        --writer.Indent;
      }
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.MoveTableOperation" />.
    /// </summary>
    /// <param name="moveTableOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      MoveTableOperation moveTableOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<MoveTableOperation>(moveTableOperation, nameof (moveTableOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("MoveTable(name := ");
      writer.Write(this.Quote(moveTableOperation.Name));
      writer.Write(", newSchema := ");
      writer.Write(string.IsNullOrWhiteSpace(moveTableOperation.NewSchema) ? "Nothing" : this.Quote(moveTableOperation.NewSchema));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.MoveProcedureOperation" />.
    /// </summary>
    /// <param name="moveProcedureOperation">The operation to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    protected virtual void Generate(
      MoveProcedureOperation moveProcedureOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<MoveProcedureOperation>(moveProcedureOperation, nameof (moveProcedureOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("MoveStoredProcedure(name := ");
      writer.Write(this.Quote(moveProcedureOperation.Name));
      writer.Write(", newSchema := ");
      writer.Write(string.IsNullOrWhiteSpace(moveProcedureOperation.NewSchema) ? "Nothing" : this.Quote(moveProcedureOperation.NewSchema));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.RenameTableOperation" />.
    /// </summary>
    /// <param name="renameTableOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      RenameTableOperation renameTableOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<RenameTableOperation>(renameTableOperation, nameof (renameTableOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("RenameTable(name := ");
      writer.Write(this.Quote(renameTableOperation.Name));
      writer.Write(", newName := ");
      writer.Write(this.Quote(renameTableOperation.NewName));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.RenameProcedureOperation" />.
    /// </summary>
    /// <param name="renameProcedureOperation">The operation to generate code for.</param>
    /// <param name="writer">Text writer to add the generated code to.</param>
    protected virtual void Generate(
      RenameProcedureOperation renameProcedureOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<RenameProcedureOperation>(renameProcedureOperation, nameof (renameProcedureOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("RenameStoredProcedure(name := ");
      writer.Write(this.Quote(renameProcedureOperation.Name));
      writer.Write(", newName := ");
      writer.Write(this.Quote(renameProcedureOperation.NewName));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.RenameColumnOperation" />.
    /// </summary>
    /// <param name="renameColumnOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      RenameColumnOperation renameColumnOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<RenameColumnOperation>(renameColumnOperation, nameof (renameColumnOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("RenameColumn(table := ");
      writer.Write(this.Quote(renameColumnOperation.Table));
      writer.Write(", name := ");
      writer.Write(this.Quote(renameColumnOperation.Name));
      writer.Write(", newName := ");
      writer.Write(this.Quote(renameColumnOperation.NewName));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.RenameIndexOperation" />.
    /// </summary>
    /// <param name="renameIndexOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(
      RenameIndexOperation renameIndexOperation,
      IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<RenameIndexOperation>(renameIndexOperation, nameof (renameIndexOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("RenameIndex(table := ");
      writer.Write(this.Quote(renameIndexOperation.Table));
      writer.Write(", name := ");
      writer.Write(this.Quote(renameIndexOperation.Name));
      writer.Write(", newName := ");
      writer.Write(this.Quote(renameIndexOperation.NewName));
      writer.WriteLine(")");
    }

    /// <summary>
    /// Generates code to perform a <see cref="T:System.Data.Entity.Migrations.Model.SqlOperation" />.
    /// </summary>
    /// <param name="sqlOperation"> The operation to generate code for. </param>
    /// <param name="writer"> Text writer to add the generated code to. </param>
    protected virtual void Generate(SqlOperation sqlOperation, IndentedTextWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<SqlOperation>(sqlOperation, nameof (sqlOperation));
      System.Data.Entity.Utilities.Check.NotNull<IndentedTextWriter>(writer, nameof (writer));
      writer.Write("Sql(");
      writer.Write(this.Quote(sqlOperation.Sql));
      if (sqlOperation.SuppressTransaction)
        writer.Write(", suppressTransaction := True");
      writer.WriteLine(")");
    }

    /// <summary>
    /// Removes any invalid characters from the name of an database artifact.
    /// </summary>
    /// <param name="name"> The name to be scrubbed. </param>
    /// <returns> The scrubbed name. </returns>
    protected virtual string ScrubName(string name)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      name = new Regex("[^\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Cf}\\p{Pc}\\p{Lm}]").Replace(name, string.Empty);
      using (VBCodeProvider vbCodeProvider = new VBCodeProvider())
      {
        if (char.IsLetter(name[0]) || name[0] == '_')
        {
          if (vbCodeProvider.IsValidIdentifier(name))
            goto label_7;
        }
        name = "_" + name;
      }
label_7:
      return name;
    }

    /// <summary>
    /// Gets the type name to use for a column of the given data type.
    /// </summary>
    /// <param name="primitiveTypeKind"> The data type to translate. </param>
    /// <returns> The type name to use in the generated migration. </returns>
    protected virtual string TranslateColumnType(PrimitiveTypeKind primitiveTypeKind)
    {
      switch (primitiveTypeKind)
      {
        case PrimitiveTypeKind.Int16:
          return "Short";
        case PrimitiveTypeKind.Int32:
          return "Int";
        case PrimitiveTypeKind.Int64:
          return "Long";
        default:
          return Enum.GetName(typeof (PrimitiveTypeKind), (object) primitiveTypeKind);
      }
    }

    /// <summary>
    /// Quotes an identifier using appropriate escaping to allow it to be stored in a string.
    /// </summary>
    /// <param name="identifier"> The identifier to be quoted. </param>
    /// <returns> The quoted identifier. </returns>
    protected virtual string Quote(string identifier) => "\"" + identifier + "\"";
  }
}
