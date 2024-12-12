using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//////////////////////
[Serializable]
public class FailCallbackError
{
    public bool success;
    public string msg;
}

//////////////////////

[Serializable]
public class DataGeoIP
{
    public string ip;
    public string country_code;
    public string country_name;
    public string region_code;
    public string region_name;
    public string city;
    public string zip_code;
    public string time_zone;
    public string latitude;
    public string longitude;
}

//////////////////////

[Serializable]
public class DataAuthResponse
{
    public bool success;
    public string jwt;
    public DataUserMailIDResponse data;
}
[Serializable]
public class DataUserMailIDResponse
{
    public UserMailIDResponse user;
}

[Serializable]
public class UserMailIDResponse
{
    public string email;
    public string uuid;
    public string full_name;
    public string username;
    public int notif_count;
}

//////////////////////

[Serializable]
public class UsernameResponse
{

    public bool success;
    public string msg;

}

//////////////////////

[Serializable]
public class FetchNFTServerResponse
{
    public bool success;
    public string msg;
}

//////////////////////

[Serializable]
public class AddWalletIDServerResponse
{
    public bool success;
    public string msg;
}

//////////////////////

[Serializable]
public class GetNftByIndexServerResponse
{
    public bool success;
    public string msg;
}

//////////////////////

[Serializable]
public class GetNftByUuidServerResponse
{
    public bool success;
    public string msg;
}


////////////////////////

[Serializable]
public class UserResponse
{
    public int id;
    public string username;
    public string full_name;
    public string email;
    public string timezone;
    public string uuid;
    public int notif_count;
    public string token;
}


[Serializable]
public class UserDataResponse
{
    public UserResponse user;
}

/////////////////////////

[Serializable]
public class GetUserResponse
{
    public bool success;
    public UserDataResponse data;
}

////////////////////////

[Serializable]
public class UpdatePasswordResponse
{
    public bool success;
    public string msg;
}

////////////////////////


[Serializable]
public class AllRoomInfos
{
    public string page;
    public int perPage;
    public int totalCount;
}

[Serializable]
public class RoomInfos
{
    public string room_id;
    public string room_name;
    public string category_type;
    public int rank_points;
    public int distance;
    public int total_person;
    public int axp_points;
    public int participantCount;
    public int entry_fuel_fee;
}


[Serializable]
public class AllRoomResponse
{
    public bool success;
    public List<RoomInfos> data;
    public AllRoomInfos meta;

}

////////////////////////

[Serializable]
public class RegisterRoomResponse
{
    public bool success;
    public bool is_full;
}

///////////////////////////

[Serializable]
public class RoomID
{
    public string room_id;
    public bool is_full;
}

////////////////////////////////

[Serializable]
public class CarAttribute
{
    public string trait_type;
    public string value;
    public int? level;
    public int? xp;
}


[Serializable]
public class User
{
    public string username;
    public string room_name;
    public int car_id;
    public string uuid;
    public string name;
    public List<CarAttribute> attributes;
    public string image;
    public int event_id;
}

[Serializable]
public class Participants
{
    public bool success;
    public List<User> users;
}
////////////////////////

[Serializable]
public class Det
{
    public int engine_id;
    public string name;
    public string image;
}

[Serializable]
public class Root
{
    public bool success;
    public List<Det> users;
}

///////////////////////

[Serializable]
public class MissionDetail
{
    public int id;
    public string uuid;
    public int engine_id;
    public string image;
    public int time_hour;
    public int time_second;
    public int earned_value;
    public string createdAt;
    public string updatedAt;
    public bool is_active;
    public bool is_done;
    public int end_time_unix;
    public string end_time_date;
    public int start_time_unix;
    public string start_time_date;
    public int mission_user_id;
}

[Serializable]
public class Details
{
    public bool success;
    public int start_time_unix;
    public string start_time_date;
    public bool checkHasMission;
    public bool isRewardEnabled;
    public List<MissionDetail> data;
}

///////////////////////

[Serializable]
public class FuelDetail
{
    public int engine_id;
    public int current_fuel;
    public string name;
}

[Serializable]
public class AllFuels
{
    public bool success;
    public List<FuelDetail> fuelDetail;
}

/////////////////////////

[Serializable]
public class EngineSpec
{
    public int user_id;
    public int nft_id;
    public string spec_name;
    public int old_level;
    public int new_level;
}

[Serializable]
public class UpgradeEngineResponse
{
    public bool success;
    public EngineSpec data;
}

////////////////////////////////////////////

[Serializable]
public class DrivetrainSpec
{
    public int user_id;
    public int nft_id;
    public string spec_name;
    public int old_level;
    public int new_level;
}


[Serializable]
public class UpgradeDrivetrainResponse
{
    public bool success;
    public DrivetrainSpec data;
}

///////////////////////////////////////

[Serializable]
public class TurboSpec
{
    public int user_id;
    public int nft_id;
    public string spec_name;
    public int old_level;
    public int new_level;
}


[Serializable]
public class UpgradeTurboResponse
{
    public bool success;
    public TurboSpec data;
}


/////////////////////////////////////////

[Serializable]
public class CarRaceInfo
{
    public int user_id;
    public int car_id;
    public string name;
    public string rarity;
    public double engineValue;
    public double ptValue;
    public double turboValue;
    public double wheelValue;
    public double wheightValue;
    public int weight;
    public double result;
    public int counter;
    public List<int> len;
}

[Serializable]
public class RaceInfosFromServer
{
    public bool success;
    public List<CarRaceInfo> data;
}

///////////////////////////////////////////////

[Serializable]
public class GetRoomDetailResponse
{
    public int id;
    public string uuid;
    public string room_name;
    public string category_type;
    public int category_id;
    public int rank_points;
    public int axp_points;
    public int distance;
    public int total_person;
    public string wind_type;
    public string wind_value;
    public string weather;
    public bool is_finished;
    public bool is_deleted;
    public DateTime createdAt;
    public DateTime updatedAt;
}

[Serializable]
public class GetRoomDetailServerResponse
{
    public bool success;
    public GetRoomDetailResponse data;
    public int totalParticipant;
}

[Serializable]
public class CarStats
{
    public int rank_points;
    public int axp_points;
    public int engine_level;
    public int drive_train_level;
    public int turbo_level;
}

[Serializable]
public class GetCarSpecsServer
{
    public bool success;
    public CarStats data;
}


///////////////////////////////

[Serializable]
public class MessageContent
{
    public string sender ;
    public string text ;
    public string date ;
}



