import { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Image from 'react-bootstrap/Image';
import ILocalidadData from '../../domain/Localidades/ILocalidadData';
import ITipoCasa from '../../domain/TiposCasas/ITipoCasa';
import IItemQuery from '../../domain/Casas/IItemQuery';
import GetItems from '../../infrestructure/services/services';
import Spinner from '../../components/spinners/spinner';

function Inmuebles() {
  const [localidad, setLocalidad] = useState<ILocalidadData[]>([]);
  const [vivienda, setVivienda] = useState<ITipoCasa[]>([]);
  const [newErrors, setErrors] = useState("");
  const [divError, setDivError] = useState(false);

  // 🟣 Estado para los valores del formulario
  const [formData, setFormData] = useState({
    ubicacion: '',
    tamanio: '',
    habitaciones: '',
    banios: '',
    tipo: '',
    antiguedad: ''
  });

  const [formDataOut, setFormDataOut] = useState({ precio: 0 });

  useEffect(() => {
    loadTipoUbicacion();
    loadTipoCasa();
  }, []);

  const loadTipoUbicacion = async () => {
    GetItems("https://localhost:7047/api/Localidades")
      .then((data) => {
        setLocalidad(data);
      }).catch(() => {
        console.error("Error en la petición");
      });
  };

  const loadTipoCasa = async () => {
    GetItems("https://localhost:7047/api/TiposCasas")
      .then((data) => {
        setVivienda(data);
      }).catch(() => {
        console.error("Error en la petición");
      });
  };

  const validateInpus = (): boolean => {
    setErrors("");
    setDivError(false);

    if (!formData.ubicacion || formData.ubicacion === "Seleccionar") {
      setErrors("Debes seleccionar una ubicación.");
      setDivError(true);
      return false;
    }

    if (!formData.tamanio || isNaN(Number(formData.tamanio)) || Number(formData.tamanio) <= 0) {
      setErrors("Debes ingresar un tamaño.");
      setDivError(true);
      return false;
    }

    if (!formData.habitaciones || isNaN(Number(formData.habitaciones)) || Number(formData.habitaciones) <= 0) {
      setErrors("Debes ingresar un número de habitaciones.");
      setDivError(true);
      return false;
    }

    if (!formData.banios || isNaN(Number(formData.banios)) || Number(formData.banios) <= 0) {
      setErrors("Debes ingresar un número de baños.");
      setDivError(true);
      return false;
    }

    if (!formData.tipo || formData.tipo === "Seleccionar") {
      setErrors("Debes seleccionar un tipo de casa.");
      setDivError(true);
      return false;
    }

    if (!formData.antiguedad || isNaN(Number(formData.antiguedad)) || Number(formData.antiguedad) <= 0) {
      setErrors("Debes ingresar un número de antigüedad.");
      setDivError(true);
      return false;
    }

    return true;
  }

  const getCalculatePrice = async (itemsQuery: IItemQuery) => {
    if (validateInpus()) {
      const urlDatas = `UbicacionId=${itemsQuery.ubicacionid}&TamanioM2=${itemsQuery.tamaniom2}&Habitaciones=${itemsQuery.habitaciones}&Banios=${itemsQuery.banios}&TipoViviendaId=${itemsQuery.tipocasaid}&Antiguedad=${itemsQuery.antiguedad}`;
      const urlTotal = `https://localhost:7047/api/Casas/getcasaprediction?${urlDatas}`;
      return GetItems(urlTotal)
        .then((data) => {
          console.log(data);
          return data;
        }).catch(() => {
          console.error("Error en la petición");
          return 0;
        });
    }
  }

  const handleChange: React.ChangeEventHandler<FormControlElement> = (e) => {
    const { name, value } = e.currentTarget;
    setFormData({ ...formData, [name]: value });
  };


  // 🟣 Reset
  const limpiarFormulario = () => {
    setFormData({
      ubicacion: '',
      tamanio: '',
      habitaciones: '',
      banios: '',
      tipo: '',
      antiguedad: ''
    });
    window.location.reload();
  };

  // 🟣 Simular cálculo (podrías usar formData directamente)
  const calculatePrice = () => {
    const ubis = localidad.filter(x => x.ubicacion === formData.ubicacion);
    const tipus = vivienda.filter(x => x.tipo === formData.tipo);

    const itemsQuery: IItemQuery = {
      ubicacionid: ubis[0].localidadId, 
      tamaniom2: formData.tamanio, 
      habitaciones: formData.habitaciones,
      banios: formData.banios,
      tipocasaid: tipus[0].tipoCasaId,
      antiguedad: formData.antiguedad
    };

    getCalculatePrice(itemsQuery)
      .then((data) => {
        setFormDataOut({ ...formData, precio: data.precio });
      }).catch(() => {
        setFormDataOut({ ...formData, precio: 0 });
      });
  };

  return (
    <>
      {
        localidad.length === 0 || vivienda.length === 0 ? 
          <Container>
            <Row>
              <Col style={{ padding: "24px", textAlign: "center" }}>
                <Spinner animation="border" variant="warning" />
              </Col>
            </Row>          
          </Container>
        :
        <Container>
          <Row>
            <Col style = {{ padding: "7px", textAlign: "center" }}><h2>Cotización de Inmuebles Mediante Inteligencia Artificial</h2></Col >
          </Row >
          <Row>
            <Col style={{ textAlign: "center" }}>
              <Image src="/src/assets/casa_portada.png" thumbnail style={{ width: "50%", height: "80%" }} />
            </Col>
          </Row>
          <Row>
            <Col>
              <InputGroup className="mb-3">
                <InputGroup.Text>Ubicación:</InputGroup.Text>
                  <Form.Select name="ubicacion" value={formData.ubicacion} onChange={handleChange}>
                  <option value="">Seleccionar</option>
                    {localidad.map((itemData, idx) =>
                      <option key={idx} value={itemData.localidadid}>{itemData.ubicacion}</option>
                  )}
                </Form.Select>
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Tamaño en Mts. Cuadrados:</InputGroup.Text>
                <Form.Control type="number" name="tamanio" value={formData.tamanio} onChange={handleChange} />
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Habitaciones:</InputGroup.Text>
                <Form.Control type="number" name="habitaciones" value={formData.habitaciones} onChange={handleChange} />
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Baños:</InputGroup.Text>
                <Form.Control type="number" name="banios" value={formData.banios} onChange={handleChange} />
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Tipo de Vivienda:</InputGroup.Text>
                <Form.Select name="tipo" value={formData.tipo} onChange={handleChange}>
                  <option value="">Seleccionar</option>
                   {vivienda.map((itemData, idx) =>
                     <option key={idx} value={itemData.tipocasaid}>{itemData.tipo}</option>
                  )}
                </Form.Select>
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Antigüedad:</InputGroup.Text>
                <Form.Control type="number" name="antiguedad" value={formData.antiguedad} onChange={handleChange} />
              </InputGroup>

              <InputGroup className="mb-3">
                <InputGroup.Text>Valor de Cotización:</InputGroup.Text>
                <Form.Control type="number" name="precio" value={formDataOut.precio} disabled placeholder="Valor de Cotización $" />
              </InputGroup>

              {divError && (
                <div className="row">
                  <div className="col-12">
                    <p style={{ color: "red" }}>{newErrors}</p>
                  </div>
                </div>
              )}

              <ButtonGroup>
                <span style={{ padding: "5px" }}>
                  <Button variant="success" onClick={calculatePrice}>Calcular</Button>
                </span>
                <span style={{ padding: "5px" }}>
                  <Button variant="success" onClick={limpiarFormulario}>Limpiar</Button>
                </span>
              </ButtonGroup>
            </Col>
          </Row>
        </Container>
      }          
    </>    
  );
}

export default Inmuebles;
