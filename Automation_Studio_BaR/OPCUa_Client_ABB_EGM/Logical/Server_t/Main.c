/**
 * MIT License
 * Copyright(c) 2020 Roman Parak
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/**
 * Author   : Roman Parak
 * Email    : Roman.Parak@outlook.com
 * File Name: Main.c
 * Github   : https://github.com/rparak
 */


/** < Include B&R Automation libraries (declarations for B&R ANSI C extensions) */
#include <bur/plctypes.h>

#ifdef _DEFAULT_INCLUDES
	#include <AsDefault.h>
#endif

// ABB Control // STRUCT // 
_LOCAL struct abb_m abb_control;

/**
 * Program Intitialization
 */
void _INIT ProgramInit(void)
{
	/// ABB CONTROL {Command - Joint}
	abb_control.Command.joint.start = 0;
	abb_control.Command.joint.stop = 0;
	abb_control.Command.joint.default_parameters = 0;
	abb_control.Command.joint.is_started = 0;
	/// ABB CONTROL {Joint - Position (WRITE)}
	abb_control.Joint.Write.J1 = 0;
	abb_control.Joint.Write.J2 = 0;
	abb_control.Joint.Write.J3 = 0;
	abb_control.Joint.Write.J4 = 0;
	abb_control.Joint.Write.J5 = 90;
	abb_control.Joint.Write.J6 = 0;
	/// ABB CONTROL {Joint - Position (WRITE)} 
	abb_control.Joint.Read.J1 = 0;
	abb_control.Joint.Read.J2 = 0;
	abb_control.Joint.Read.J3 = 0;
	abb_control.Joint.Read.J4 = 0;
	abb_control.Joint.Read.J5 = 0;
	abb_control.Joint.Read.J6 = 0;
	/// ABB CONTROL {Command - Cartesian}
	abb_control.Command.cartesian.start = 0;
	abb_control.Command.cartesian.stop = 0;
	abb_control.Command.cartesian.default_parameters = 0;
	abb_control.Command.cartesian.is_started = 0;
	/// ABB CONTROL {Cartesian - Position (WRITE)}
	abb_control.Cartesian.Write.X = 302;
	abb_control.Cartesian.Write.Y = 0;
	abb_control.Cartesian.Write.Z = 558;
	abb_control.Cartesian.Write.RX = 180;
	abb_control.Cartesian.Write.RY = 0;
	abb_control.Cartesian.Write.RZ = 180;
	/// ABB CONTROL {Cartesian - Position (WRITE)}
	abb_control.Cartesian.Read.X = 0;
	abb_control.Cartesian.Read.Y = 0;
	abb_control.Cartesian.Read.Z = 0;
	abb_control.Cartesian.Read.RX = 0;
	abb_control.Cartesian.Read.RY = 0;
	abb_control.Cartesian.Read.RZ = 0;
}

/**
 * Program Cyclic 
 * 
 * Duration (Cycle Time): 10000 [µs] 
 * Tolerance            : 10000 [µs]
 */
void _CYCLIC ProgramCyclic(void)
{
	/**
	 START CONTROL {JOINT}
	 */
	if(abb_control.Command.joint.start == 1){
		abb_control.Command.joint.is_started      = 1;
		abb_control.Command.cartesian.is_started  = 0;
	}else if(abb_control.Command.joint.stop == 1){
		abb_control.Command.joint.is_started = 0;
	}
	
	/**
	START READ {JOINT}
	*/
	if(abb_control.Command.joint.start_read_data == 1){
		abb_control.Command.joint.is_read  = 1;
	}else if(abb_control.Command.joint.stop_read_data == 1){
		abb_control.Command.joint.is_read  = 0;
	}
	
	/**
	START CONTROL {CARTESIAN}
	*/
	if(abb_control.Command.cartesian.start == 1){
		abb_control.Command.cartesian.is_started  = 1;
		abb_control.Command.joint.is_started      = 0;
	}else if(abb_control.Command.cartesian.stop == 1){
		abb_control.Command.cartesian.is_started = 0;
	}

	/**
	START READ {CARTESIAN}
	*/
	if(abb_control.Command.cartesian.start_read_data == 1){
		abb_control.Command.cartesian.is_read  = 1;
	}else if(abb_control.Command.cartesian.stop_read_data == 1){
		abb_control.Command.cartesian.is_read  = 0;
	}
	
	/**
	DEFAULT PARAM. {JOINT}
	*/
	if(abb_control.Command.joint.default_parameters == 1){
		abb_control.Joint.Write.J1 = 0;
		abb_control.Joint.Write.J2 = 0;
		abb_control.Joint.Write.J3 = 0;
		abb_control.Joint.Write.J4 = 0;
		abb_control.Joint.Write.J5 = 90;
		abb_control.Joint.Write.J6 = 0;
	}
	
	/**
	DEFAULT PARAM. {CARTESIAN}
	*/
	if(abb_control.Command.cartesian.default_parameters == 1){
		abb_control.Cartesian.Write.X = 302;
		abb_control.Cartesian.Write.Y = 0;
		abb_control.Cartesian.Write.Z = 558;
		abb_control.Cartesian.Write.RX = 180;
		abb_control.Cartesian.Write.RY = 0;
		abb_control.Cartesian.Write.RZ = 180;
	}
}

