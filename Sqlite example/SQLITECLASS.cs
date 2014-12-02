﻿using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;

public class SQLITECLASS : MonoBehaviour
{
    //----------------------------------------------------------------------------------------
    // Author: Zumwalt
    // Date: 8/30/2010
    // Unity Version: U3B6
    // Known issues: None
    //----------------------------------------------------------------------------------------
    // There are different ways to do this, both of these work however
    // at the time of the writing of this code, the Password feature
    // does NOT work in U3B6, it might or might not work in future releases
    // or when Unity gets out of Beta for U3
    //----------------------------------------------------------------------------------------
    //string constr = "URI=file:gamedata.db,Version=3,Compress=True";
    string constr = "Data Source=gamedata.db;Version=3;Password=myPassword;Compress=True";
    NetworkPlayer p;
        p = Network.player;
        Debug.Log("CreateDatabase being called");
        CreateDatabase();
        Debug.Log("InsertRecord being called");
        InsertRecord();
        ReadAllRecords();
        Debug.Log("UpdateAllRecords being called");
        UpdateAllRecords();
        ReadAllRecords();
        Debug.Log("DeleteAllRecords being called");
        DeleteAllRecords();
        ReadAllRecords();

        Debug.Log("Done with SQLITE Test");
    // http://www.sqlite.org/

    void CreateDatabase()
    {
        // example query, but this query creates the database which will be housed in our gamedata.db file if that file
        // does not exist, it will create the file automatically for us and there is nothing to write to make that happen
        string sql = "CREATE TABLE IF NOT EXISTS gamedata (pk INTEGER NOT NULL PRIMARY KEY, guid varchar(64),lastIP varchar(32)); ";
        try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                Debug.Log("executing create using executenonquery");
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void ReadAllRecords()
    {
        string sql = "SELECT * FROM gamedata;";
        try
        {
            // Create a new connection using our connection string defined at the start of the class
            // NOTE: in the reading, we are using the reader to close the connection, so using statement
            // would be redundant and attempt to a double close of the object which is no longer valid
            SqliteConnection con = new SqliteConnection(constr);
            con.Open();
            // create a command object, using our sql string above against our connection object
            SqliteCommand cmd = new SqliteCommand(sql, con);
            SqliteDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (rdr.HasRows == true)
            {
                while (rdr.Read())
                {
                    Debug.Log("Record information in database");
                    Debug.Log("Primary Key: " + rdr["pk"].ToString());
                    Debug.Log("GUID: " + rdr["guid"].ToString());
                    Debug.Log("IP: " + rdr["lastip"].ToString());
                    Debug.Log("----------------------------------");
                }
            }
            else
            {
                Debug.Log("Database contains no records in gamedata table");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void DeleteAllRecords()
    {
        // this statement purges all records from the gamedata table, to only delete 1 record
        // use a WHERE clause
        string sql = "DELETE FROM gamedata";
        try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void UpdateAllRecords()
    {
        try
        {
            // this is VERY generic, this will update ALL records and replace the GUID to this value
            // use WHERE clauses to update just one record by what ever logic you want
            string sql = "UPDATE gamedata SET GUID='1234567890';";
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                Debug.Log("updating the record with some junk data for test UPDATE");
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void InsertRecord()
    {
        // an insert statement that uses command parameters, NOTE, see the NULL, this is because the first value
        // is our primary key, this is an INTEGER, Automatically incremented, and not null, but we do nothing with it
        // however it is in the way and has to be used in the query, so we just say we are sending nothing and trusting
        // SQLLite to know what to do with it
        string sql = "INSERT INTO gamedata (pk, guid, lastip) Values (NULL, ?, ?); ";

        try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // we shall keep things clean, so lets use parameters to store our values and pass up into our table
                SqliteParameter p0 = new SqliteParameter();
                p0.ParameterName = "@pguid";
                p0.DbType = DbType.String;
                p0.Value = p.guid;

                SqliteParameter p1 = new SqliteParameter();
                p1.ParameterName = "@plastip";
                p1.DbType = DbType.String;
                p1.Value = p.ipAddress;

                try
                {
                    // create a command object, using our sql string above against our connection object
                    SqliteCommand cmd = new SqliteCommand(sql, con);
                    // add our parameters to the command object
                    cmd.Parameters.Add(p0);
                    cmd.Parameters.Add(p1);
                    // execute the command
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }