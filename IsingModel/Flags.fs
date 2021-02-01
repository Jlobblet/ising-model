module IsingModel.Flags

type ``Average energy and magnetisation`` =
    | ``Plot average energy and magnetisation``
    | ``Do not plot average energy and magnetisation``

type ``Show state during simulation`` =
    | ``Show the state during simulation``
    | ``Do not show the state during simulation``

type ``Temperature dependence of average system energy and magnetisation`` =
    | ``Plot temperature dependence of average system energy and magnetisation``
    | ``Do not plot temperature dependence of average system energy and magnetisation``

type ``System energy and magnetisation as a function of a varying external magnetic field`` =
    | ``Plot system energy and magnetisation as a function of a varying external magnetic field``
    | ``Do not plot system energy and magnetisation as a function of a varying external magnetic field``

type ``External applied magnetic field strength dependence of average system energy and magnetisation`` =
    | ``Plot external applied magnetic field strength dependence of average system energy and magnetisation``
    | ``Do not plot external applied magnetic field strength dependence of average system energy and magnetisation``

let avFlag =
    ``Plot average energy and magnetisation``

let stateFlag =
    ``Do not show the state during simulation``

let tDep =
    ``Do not plot temperature dependence of average system energy and magnetisation``

let hVary =
    ``Plot system energy and magnetisation as a function of a varying external magnetic field``

let hDep =
    ``Do not plot external applied magnetic field strength dependence of average system energy and magnetisation``
