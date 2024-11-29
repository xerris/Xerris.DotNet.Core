using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Xerris.DotNet.Core.Extensions;

public static class ReflectionExtensions
{
    public static IEnumerable<Type> GetImplementingTypes(this Type t, params Assembly[] targetAssemblies)
    {
        if (t == null) return [];

        var searchAssemblies =
            targetAssemblies.Length != 0 ? targetAssemblies : AppDomain.CurrentDomain.GetAssemblies();

        return searchAssemblies
            .SelectMany(s => s.GetTypes())
            .Where(tt => tt.IsClass && !tt.IsAbstract && t.IsAssignableFrom(tt));
    }

    public static IEnumerable<Assembly> GetParentAssemblies(this Assembly a)
    {
        return new StackTrace().GetFrames()
            .Where(f => f.GetMethod()?.ReflectedType != null)
            .Select(f => f.GetMethod()?.ReflectedType.Assembly)
            .Distinct().Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == a.FullName));
    }

    public static PropertyInfo GetProperty<TModel, T>(this Expression<Func<TModel, T>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static object GetValue<TModel, T>(this Expression<Func<TModel, T>> expression, T obj)
    {
        var info = expression.GetProperty();
        return info.GetValue(obj, Array.Empty<object>());
    }

    public static string NameOfProperty<TModel, T>(this Expression<Func<TModel, T>> expression)
        => expression.GetProperty().Name;

    private static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression,
        bool enforceCheck = true)
    {
        MemberExpression memberExpression = null;
        switch (expression.Body.NodeType)
        {
            case ExpressionType.Convert:
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
                break;
            }
            case ExpressionType.MemberAccess:
                memberExpression = expression.Body as MemberExpression;
                break;
        }

        if (enforceCheck && memberExpression == null)
            throw new ArgumentException("Not a member access", nameof(expression));

        return memberExpression;
    }
}