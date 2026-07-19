# App_Data Notes

`filistindb_full.sql` and `filistindb_data_only.sql` are legacy database exports kept for historical/debugging reference only.

Do not import these files into production. They may contain obsolete temporary domain values, old seed colors, and historical data snapshots. Production data should be created from EF migrations plus current admin configuration or a freshly generated sanitized dump.
