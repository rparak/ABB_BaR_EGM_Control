
TYPE
	abb_cartes_cmd : 	STRUCT 
		Read : abb_cartesian;
		Write : abb_cartesian;
	END_STRUCT;
	abb_joint_cmd : 	STRUCT 
		Read : abb_joint;
		Write : abb_joint;
	END_STRUCT;
	abb_m : 	STRUCT 
		Joint : abb_joint_cmd;
		Cartesian : abb_cartes_cmd;
		Command : abb_cmd_m;
	END_STRUCT;
	abb_cmd : 	STRUCT 
		start : BOOL;
		stop : BOOL;
		is_started : BOOL;
		default_parameters : BOOL;
		start_read_data : BOOL;
		stop_read_data : BOOL;
		is_read : BOOL;
	END_STRUCT;
	abb_cmd_m : 	STRUCT 
		joint : abb_cmd;
		cartesian : abb_cmd;
	END_STRUCT;
	abb_cartesian : 	STRUCT 
		X : LREAL;
		Y : LREAL;
		Z : LREAL;
		RX : LREAL;
		RY : LREAL;
		RZ : LREAL;
	END_STRUCT;
	abb_joint : 	STRUCT 
		J1 : LREAL;
		J2 : LREAL;
		J3 : LREAL;
		J4 : LREAL;
		J5 : LREAL;
		J6 : LREAL;
	END_STRUCT;
END_TYPE
