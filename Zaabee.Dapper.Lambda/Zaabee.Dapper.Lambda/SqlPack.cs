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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zaabee.Dapper.Lambda
{
	public class SqlPack
	{
		private static readonly List<string> SListEnglishWords = new List<string>
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
			"v", "w", "x", "y", "z",
		};

		private readonly Dictionary<string, string> _dicTableName = new Dictionary<string, string>();
		private Queue<string> _queueEnglishWords = new Queue<string>(SListEnglishWords);

		public bool IsSingleTable { get; set; }

		public List<string> SelectFields { get; set; }

		public string SelectFieldsStr => string.Join(",", SelectFields);

		public int Length => Sql.Length;

		public StringBuilder Sql { get; set; }

		public DatabaseType DatabaseType { get; set; }

		public Dictionary<string, object> DbParams { get; private set; }

		private string DbParamPrefix
		{
			get
			{
				switch (DatabaseType)
				{
					case DatabaseType.SQLite:
					case DatabaseType.SQLServer: return "@";
					case DatabaseType.MySQL: return "?";
					case DatabaseType.Oracle: return ":";
					default: return "";
				}
			}
		}

		public char this[int index]
		{
			get { return Sql[index]; }
		}

		public SqlPack()
		{
			DbParams = new Dictionary<string, object>();
			Sql = new StringBuilder();
			SelectFields = new List<string>();
		}

		public static SqlPack operator +(SqlPack sqlPack, string sql)
		{
			sqlPack.Sql.Append(sql);
			return sqlPack;
		}

		public void Clear()
		{
			SelectFields.Clear();
			Sql.Clear();
			DbParams.Clear();
			_dicTableName.Clear();
			_queueEnglishWords = new Queue<string>(SListEnglishWords);
		}

		public void AddDbParameter(object parameterValue)
		{
			if (parameterValue == null || parameterValue == DBNull.Value)
				Sql.Append(" null");
			else
			{
				var name = DbParamPrefix + "param" + DbParams.Count;
				DbParams.Add(name, parameterValue);
				Sql.Append(" " + name);
			}
		}

		public bool SetTableAlias(string tableName)
		{
			if (_dicTableName.Keys.Contains(tableName)) return false;
			_dicTableName.Add(tableName, _queueEnglishWords.Dequeue());
			return true;
		}

		public string GetTableAlias(string tableName)
		{
			if (!IsSingleTable && _dicTableName.Keys.Contains(tableName))
				return _dicTableName[tableName];

			return "";
		}

		public override string ToString()
		{
			return Sql.ToString();
		}
	}
}