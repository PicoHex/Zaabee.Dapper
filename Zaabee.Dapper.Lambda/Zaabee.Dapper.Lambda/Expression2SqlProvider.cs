#region License
/**
 * Copyright (c) 2015, 何志祥 (strangecity@qq.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * without warranties or conditions of any kind, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Linq.Expressions;
using Zaabee.Dapper.Lambda.Expression2Sql;

namespace Zaabee.Dapper.Lambda
{
	internal class Expression2SqlProvider
	{
		private static IExpression2Sql GetExpression2Sql(Expression expression)
		{
			switch (expression)
			{
				case null:
					throw new ArgumentNullException(nameof(expression), "不能为null");
				case BinaryExpression _:
					return new BinaryExpression2Sql();
				case BlockExpression _:
					throw new NotImplementedException("未实现的BlockExpression2Sql");
				case ConditionalExpression _:
					throw new NotImplementedException("未实现的ConditionalExpression2Sql");
				case ConstantExpression _:
					return new ConstantExpression2Sql();
				case DebugInfoExpression _:
					throw new NotImplementedException("未实现的DebugInfoExpression2Sql");
				case DefaultExpression _:
					throw new NotImplementedException("未实现的DefaultExpression2Sql");
				case DynamicExpression _:
					throw new NotImplementedException("未实现的DynamicExpression2Sql");
				case GotoExpression _:
					throw new NotImplementedException("未实现的GotoExpression2Sql");
				case IndexExpression _:
					throw new NotImplementedException("未实现的IndexExpression2Sql");
				case InvocationExpression _:
					throw new NotImplementedException("未实现的InvocationExpression2Sql");
				case LabelExpression _:
					throw new NotImplementedException("未实现的LabelExpression2Sql");
				case LambdaExpression _:
					throw new NotImplementedException("未实现的LambdaExpression2Sql");
				case ListInitExpression _:
					throw new NotImplementedException("未实现的ListInitExpression2Sql");
				case LoopExpression _:
					throw new NotImplementedException("未实现的LoopExpression2Sql");
				case MemberExpression _:
					return new MemberExpression2Sql();
				case MemberInitExpression _:
					throw new NotImplementedException("未实现的MemberInitExpression2Sql");
				case MethodCallExpression _:
					return new MethodCallExpression2Sql();
				case NewArrayExpression _:
					return new NewArrayExpression2Sql();
				case NewExpression _:
					return new NewExpression2Sql();
				case ParameterExpression _:
					throw new NotImplementedException("未实现的ParameterExpression2Sql");
				case RuntimeVariablesExpression _:
					throw new NotImplementedException("未实现的RuntimeVariablesExpression2Sql");
				case SwitchExpression _:
					throw new NotImplementedException("未实现的SwitchExpression2Sql");
				case TryExpression _:
					throw new NotImplementedException("未实现的TryExpression2Sql");
				case TypeBinaryExpression _:
					throw new NotImplementedException("未实现的TypeBinaryExpression2Sql");
				case UnaryExpression _:
					return new UnaryExpression2Sql();
				default:
					throw new NotImplementedException("未实现的Expression2Sql");
			}
		}

		public static void Update(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Update(expression, sqlPack);
		}

		public static void Select(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Select(expression, sqlPack);
		}

		public static void Join(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Join(expression, sqlPack);
		}

		public static void Where(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Where(expression, sqlPack);
		}

		public static void In(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).In(expression, sqlPack);
		}

		public static void GroupBy(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).GroupBy(expression, sqlPack);
		}

		public static void OrderBy(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).OrderBy(expression, sqlPack);
		}

		public static void Max(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Max(expression, sqlPack);
		}

		public static void Min(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Min(expression, sqlPack);
		}

		public static void Avg(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Avg(expression, sqlPack);
		}

		public static void Count(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Count(expression, sqlPack);
		}

		public static void Sum(Expression expression, SqlPack sqlPack)
		{
			GetExpression2Sql(expression).Sum(expression, sqlPack);
		}
	}
}