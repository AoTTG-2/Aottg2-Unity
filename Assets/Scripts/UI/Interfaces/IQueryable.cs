using System.Collections.Generic;

namespace Utility
{
    public interface IQueryable
    {
        /// <summary>
        /// Filters objects based on the query string.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of results matching the query.</returns>
        List<string> Query(string query);

        /// <summary>
        /// Returns the full list of queryable objects.
        /// </summary>
        List<string> GetAll();
    }
}